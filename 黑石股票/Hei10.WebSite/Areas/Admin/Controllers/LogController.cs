using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Hei10.Domain.Enums;
using Hei10.Domain.Repositories;
using Hei10.Web.Framework.Controllers;
using Hei10.Web.Framework.Security;
using Hei10.WebSite.Areas.Admin.Models;
using log4net;
using log4net.Appender;
using PagedList;
using Hei10.Domain.ViewModels;
using Hei10.Core.Utilities;
using System.Threading.Tasks;
using Hei10.Domain.Models;

namespace Hei10.WebSite.Areas.Admin.Controllers
{
    public class LogController : AdminController
    {
        public ISysOperationLogRepository SysOperationLog { get; set; }
        public ISysConfigRepository SysConfigRepository { get; set; }
        public IJobTaskRepository JobTaskRepository { get; set; }

        #region 日志
        [ValidatePage]
        public ActionResult Index(string src, string srcId, string tit, string uname, TableSource? cat, OperationType? type, DateTime? minTime, DateTime? maxTime, string orderField, string orderDirection, int pageCurrent = 1, int pageSize = 30)
        {
            ViewBag.pageCurrent = pageCurrent;
            ViewBag.pageSize = pageSize;
            ViewBag.tit = tit;
            ViewBag.src = src;
            ViewBag.srcId = srcId;
            ViewBag.uname = uname;
            ViewBag.minTime = minTime;
            ViewBag.maxTime = maxTime;
            var list = SysOperationLog.GetPagedList(pageCurrent, pageSize, orderField, orderDirection, src, srcId, cat, type, tit, uname, minTime, maxTime);
            return View(list);
        }

        [ValidatePage]
        public ActionResult LogList(string q, int pageCurrent = 1, int pageSize = 30)
        {
            ViewBag.pageCurrent = pageCurrent;
            ViewBag.pageSize = pageSize;
            DirectoryInfo d = new DirectoryInfo(SysConfigRepository.LogFilePath);
            var query = d.GetFiles("*.*").Where(file => file.Name.ToLower().EndsWith(".txt"));
            if (!string.IsNullOrEmpty(q))
            {
                query = query.Where(l => l.Name.Contains(q));
            }
            var list = query
                .Select(m => new LogModel
                {
                    Name = m.Name,
                    Length = m.Length,
                    CreatDateTime = m.CreationTime,
                    LastWriteTime = m.LastWriteTime,
                    FullName = m.FullName
                })
                .OrderByDescending(m => m.LastWriteTime)
                .ToPagedList(pageCurrent, pageSize);
            return View(list);
        }

        [ValidateButton(Buttons = SysButton.Download, ActionName = "LogList")]
        public ActionResult DownFile(string filePath, string name)//相对路径及完整文件名（有后缀）
        {
            FileStream fs = new FileStream(filePath, FileMode.Open);
            byte[] bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
            Response.ContentType = "application/octet-stream";

            Response.AddHeader("Content-Disposition", "attachment; filename=" + name);
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
            return new EmptyResult();
        }

        [ValidateButton(Buttons = SysButton.Browse, ActionName = "LogList")]
        public FileStreamResult ReadFile(string filepath)
        {
            FileStream stream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);
            reader.BaseStream.Seek(0L, SeekOrigin.Begin);
            return File(stream, "text/plain");
        }

        #endregion

        #region 后台任务

        [ValidatePage]
        public ActionResult JobTask(string name, CommonStatus? status, string orderField, string orderDirection, int pageCurrent = 1, int pageSize = 30)
        {
            var list = JobTaskRepository.GetPagedList(name, status, orderField, orderDirection, pageCurrent, pageSize);
            return View(list);
        }

        [ValidateButton(Buttons = SysButton.Delete, ActionName = "JobTask")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> JobTaskBathDelete(string ids)
        {
            var result = new JsonModel { statusCode = 300, message = "删除失败,记录不存在！", closeCurrent = false };
            if (string.IsNullOrEmpty(ids))
            {
                return Json(result);
            }
            var arr = StringUtility.ConvertToBigIntArray(ids, ',');
            if (arr == null || !arr.Any())
            {
                return Json(result);
            }
            var idString = string.Join(",", arr);
            await JobTaskRepository.BatchStatusAsync(idString, CommonStatus.Deleted);
            await SysOperationLog.Insert(TableSource.JobTask, OperationType.Delete, "", idString);
            result.statusCode = 200;
            result.message = "删除成功！";
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> JobTaskStatus(long id)
        {
            var result = new JsonModel { statusCode = 300, message = "修改状态失败,记录不存在！", closeCurrent = false };
            if (id <= 0)
            {
                return Json(result);
            }
            var model = await JobTaskRepository.GetByIdAsync(id);
            if (model == null)
            {
                return Json(result);
            }
            if (model.CommonStatus == CommonStatus.Enabled)
            {
                model.CommonStatus = CommonStatus.Disabled;
            }
            else if (model.CommonStatus == CommonStatus.Disabled)
            {
                model.CommonStatus = CommonStatus.Enabled;
            }
            await SysOperationLog.Insert(TableSource.JobTask, OperationType.Update, "", id.ToString());
            result.statusCode = 200;
            result.message = "修改状态成功！";
            return Json(result);
        }

        [ValidateButton(Buttons = SysButton.Edit, ActionName = "JobTask")]
        public async Task<ActionResult> JobTaskEdit(long id = 0)
        {
            JobTask model;
            if (id > 0)
            {
                model = await JobTaskRepository.GetByIdAsync(id);
                if (model == null)
                {
                    var json = new JsonModel { message = "记录不存在了", statusCode = 300 };
                    return Json(json, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                model = new JobTask
                {
                    CommonStatus = CommonStatus.Enabled,
                    TaskId = Guid.NewGuid()
                };
            }
            return PartialView(model);
        }

        [ValidateButton(Buttons = SysButton.Edit, ActionName = "JobTask")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> JobTaskEdit(JobTask model)
        {
            var result = new JsonModel();
            // 数据有误
            if (!ModelState.IsValid)
            {
                result.GetError(ModelState);
                return Json(result);
            }

            var type = OperationType.Insert;
            var title = "添加任务";
            //var lastRunTime = GetTaskeLastRunTime(model.CronExpressionString);
            if (model.Id > 0)
            {
                type = OperationType.Update;
                title = "修改任务";
                JobTask oModel = await JobTaskRepository.GetByIdAsync(model.Id);
                if (oModel == null)
                {
                    result.message = "记录不存在了";
                    return Json(result);
                }
                //表达式改变了重新计算下次运行时间
                if (!model.CronExpressionString.Equals(oModel.CronExpressionString, StringComparison.OrdinalIgnoreCase))
                {
                    //model.LastRunTime = lastRunTime;
                    model.IsDeleteOldTask = true;
                }
                else
                {
                    model.LastRunTime = oModel.LastRunTime;
                }
            }
            else
            {
                //model.LastRunTime = lastRunTime;
            }
            await JobTaskRepository.SaveAsync(model);
            //插入日志
            await SysOperationLog.Insert(TableSource.JobTask, type, title, model.Id);
            result.Data = model;
            result.message = "保存成功！";
            return Json(result);
        }
        #endregion

    }
}