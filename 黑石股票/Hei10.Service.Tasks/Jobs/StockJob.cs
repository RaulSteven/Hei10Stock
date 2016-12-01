using Autofac;
using Hei10.Service.Tasks.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Repositories;
using System.IO;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Hei10.Domain.Models.Stock;
using Hei10.Domain.Enums;

namespace Hei10.Service.Tasks.Jobs
{
    public class StockJob : BaseJob
    {
        public override void ExcuteTask()
        {
            try
            {
                using (var timeScope = DependencyConfig.Container.BeginLifetimeScope())
                {
                    var stockRepository = timeScope.Resolve<IStockRepository>();
                    var stockMarketRepository = timeScope.Resolve<IStockMarketRepository>();
                    var stockRecordRepository = timeScope.Resolve<IStockRecordRepository>();
                    var configRepository = timeScope.Resolve<ISysConfigRepository>();

                    var marketList = stockMarketRepository.GetListAsync().Result;
                    foreach (var market in marketList)
                    {
                        var stockList = stockRepository.GetListAsync(market.Id).Result;
                        foreach (var stock in stockList)
                        {
                            var paramList = new Dictionary<string, string>();
                            paramList.Add(market.StockKey, stock.Code);
                            paramList.Add("key", configRepository.JuheStockKey);
                            var result = SendPost(market.RequestUrl, paramList, "get");

                            var jsonObject = JObject.Parse(result);
                            var errorCode = jsonObject.Value<int>("error_code");
                            if (errorCode != 0)
                            {
                                Log.ErrorFormat("获取股票信息失败！股票编号：{0}(id:{1}),失败信息：{2}(error_code:{3}",
                                    stock.Code, stock.Id, jsonObject.Value<string>("reason"), errorCode);
                                continue;
                            }

                            var record = stockRecordRepository.GetToday(stock.Id);
                            if (record == null)
                            {
                                record = new StockRecord()
                                {
                                    StockId = stock.Id
                                };
                            }
                            var jsonResult = jsonObject.Value<JArray>("result");
                            var jsonData = jsonResult[0].Value<JObject>("data");
                            record.OpenPrice = jsonData.Value<decimal>(market.OpenPriceKey);
                            record.FormPrice = jsonData.Value<decimal>(market.FormPriceKey);
                            record.MaxPrice = jsonData.Value<decimal>(market.MaxPriceKey);
                            record.MinPrice = jsonData.Value<decimal>(market.MinPriceKey);
                            //MTR: MAX(MAX((HIGH - LOW), ABS(REF(CLOSE, 1) - HIGH)), ABS(REF(CLOSE, 1) - LOW));
                            //ATR: MA(MTR, N);

                            record.MTR = Math.Max(Math.Max(record.MaxPrice - record.MinPrice, Math.Abs(record.FormPrice - record.MaxPrice)),
                                Math.Abs(record.FormPrice - record.MinPrice));
                            record.CommonStatus = CommonStatus.Enabled;
                            stockRecordRepository.SaveAsync(record).Wait();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("股票信息获取报错！", ex);
            }
           
        }

        /// <summary>
        /// Http (GET/POST)
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="method">请求方法</param>
        /// <returns>响应内容</returns>
        private string SendPost(string url, IDictionary<string, string> parameters, string method)
        {
            if (method.ToLower() == "post")
            {
                HttpWebRequest req = null;
                HttpWebResponse rsp = null;
                System.IO.Stream reqStream = null;
                try
                {
                    req = (HttpWebRequest)WebRequest.Create(url);
                    req.Method = method;
                    req.KeepAlive = false;
                    req.ProtocolVersion = HttpVersion.Version10;
                    req.Timeout = 5000;
                    req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                    byte[] postData = Encoding.UTF8.GetBytes(BuildQuery(parameters, "utf8"));
                    reqStream = req.GetRequestStream();
                    reqStream.Write(postData, 0, postData.Length);
                    rsp = (HttpWebResponse)req.GetResponse();
                    Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
                    return GetResponseAsString(rsp, encoding);
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                finally
                {
                    if (reqStream != null) reqStream.Close();
                    if (rsp != null) rsp.Close();
                }
            }
            else
            {
                //创建请求
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "?" + BuildQuery(parameters, "utf8"));

                //GET请求
                request.Method = "GET";
                request.ReadWriteTimeout = 5000;
                request.ContentType = "text/html;charset=UTF-8";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));

                //返回内容
                string retString = myStreamReader.ReadToEnd();
                return retString;
            }
        }

        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <param name="encode"></param>
        /// <returns>URL编码后的请求数据</returns>
        private string BuildQuery(IDictionary<string, string> parameters, string encode)
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;
            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name))//&& !string.IsNullOrEmpty(value)
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }
                    postData.Append(name);
                    postData.Append("=");
                    if (encode == "gb2312")
                    {
                        postData.Append(HttpUtility.UrlEncode(value, Encoding.GetEncoding("gb2312")));
                    }
                    else if (encode == "utf8")
                    {
                        postData.Append(HttpUtility.UrlEncode(value, Encoding.UTF8));
                    }
                    else
                    {
                        postData.Append(value);
                    }
                    hasParam = true;
                }
            }
            return postData.ToString();
        }

        /// <summary>
        /// 把响应流转换为文本。
        /// </summary>
        /// <param name="rsp">响应流对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应文本</returns>
        private string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            System.IO.Stream stream = null;
            StreamReader reader = null;
            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);
                return reader.ReadToEnd();
            }
            finally
            {
                // 释放资源
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }
        }
    }
}
