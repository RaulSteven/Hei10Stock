using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hei10.Domain.Models.Stock;
using Hei10.Domain.Entityframework;
using Hei10.Web.Framework.Controllers;
using Hei10.Domain.Repositories;
using System.Threading.Tasks;
using Hei10.Web.Framework.Security;
using Hei10.Domain.Enums;
using Hei10.WebSite.Areas.Admin.Models;
using Hei10.Domain.ViewModels;
using AutoMapper;
using Hei10.Core.Extensions;

namespace Hei10.WebSite.Areas.Admin.Controllers
{
    public class StockController : AdminController
    {
        public IStockRepository StockRepository { get; set; }
        public IStockMarketRepository StockMarketRepository { get; set; }
        public IStockRecordRepository StockRecordRepository { get; set; }
        public ISysOperationLogRepository LogRepository { get; set; }

        #region 股票市场
        [ValidatePage]
        public async Task<ActionResult> MarketList()
        {
            var list = await StockMarketRepository.GetListAsync();
            return View(list);
        }

        [ValidateButton(Buttons = SysButton.Edit, ActionName = "MarketList")]
        public async Task<ActionResult> MarketEdit(long id = 0)
        {
            var model = new StockMarketModel();
            if (id == 0)
            {
                return PartialView(model);
            }
            var market = await StockMarketRepository.GetEnableByIdAsync(id);
            if (market == null)
            {
                var json = new JsonModel { message = "记录不存在了", statusCode = 300 };
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            Mapper.Map(market, model);
            return PartialView(model);
        }

        [ValidateButton(Buttons = SysButton.Edit, ActionName = "MarketList")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MarketEdit(StockMarketModel model)
        {
            var result = new JsonModel();
            if (!ModelState.IsValid)
            {
                result.GetError(ModelState);
                return Json(result);
            }

            StockMarket market = null;
            var operationType = OperationType.Update;
            if (model.Id > 0)
            {
                operationType = OperationType.Update;
                market = await StockMarketRepository.GetEnableByIdAsync(model.Id);
                if (market == null)
                {
                    result.statusCode = 300;
                    result.message = "该条数据不存在，请刷新重试！";
                    return Json(result);
                }
            }
            else
            {
                market = new StockMarket();
            }
            Mapper.Map(model, market);
            market.CommonStatus = CommonStatus.Enabled;
            await StockMarketRepository.SaveAsync(market);
            await LogRepository.Insert(TableSource.StockMarket, operationType, "", market.Id);
            result.Data = market;
            result.message = "保存成功！";
            return Json(result);
        }

        [ValidateButton(Buttons = SysButton.Delete, ActionName = "MarketList")]
        public async Task<ActionResult> MarketDelete(string ids)
        {
            var result = new JsonModel { statusCode = 300, message = "删除失败,记录不存在！", closeCurrent = false };
            if (string.IsNullOrEmpty(ids))
            {
                return Json(result);
            }
            var list = await StockMarketRepository.BatchDeleteAsync(ids);
            if (list == null)
            {
                return Json(result);
            }
            var msg = string.Join(",", list.Select(m => m.Name).ToArray()).ToEllipsis(100);
            await LogRepository.Insert(TableSource.StockMarket, OperationType.Delete, string.Format("批量删除{0}等股票市场", msg), ids);
            result.statusCode = 200;
            result.message = "删除成功！";
            return Json(result);
        }
        #endregion

        #region 股票
        [ValidatePage]
        public ActionResult StockList(string name, string orderField, string orderDirection, int pageSize = 30, int pageIndex = 1)
        {
            ViewBag.Name = name;
            var list = StockRepository.GetList(name,orderField,orderDirection,pageSize,pageIndex);
            return View(list);
        }

        [ValidateButton(Buttons = SysButton.Edit, ActionName = "StockList")]
        public async Task<ActionResult> StockEdit(long id = 0)
        {
            var model = new StockModel()
            {
                MarketList = await StockMarketRepository.GetListAsync()
            };
            if (id == 0)
            {
                return PartialView(model);
            }
            var stock = await StockRepository.GetEnableByIdAsync(id);
            if (stock == null)
            {
                var json = new JsonModel { message = "记录不存在了", statusCode = 300 };
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            Mapper.Map(stock, model);
            return PartialView(model);
        }

        [ValidateButton(Buttons = SysButton.Edit, ActionName = "StockList")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StockEdit(StockModel model)
        {
            var result = new JsonModel();
            if (!ModelState.IsValid)
            {
                result.GetError(ModelState);
                return Json(result);
            }

            Stock stock = null;
            var operationType = OperationType.Update;
            if (model.Id > 0)
            {
                operationType = OperationType.Update;
                stock = await StockRepository.GetEnableByIdAsync(model.Id);
                if (stock == null)
                {
                    result.statusCode = 300;
                    result.message = "该条数据不存在，请刷新重试！";
                    return Json(result);
                }
            }
            else
            {
                stock = new Stock();
            }
            Mapper.Map(model, stock);
            stock.CommonStatus = CommonStatus.Enabled;
            await StockRepository.SaveAsync(stock);
            await LogRepository.Insert(TableSource.Stock, operationType, "", stock.Id);
            result.Data = stock;
            result.message = "保存成功！";
            return Json(result);
        }

        [ValidateButton(Buttons = SysButton.Delete, ActionName = "StockList")]
        public async Task<ActionResult> StockDelete(string ids)
        {
            var result = new JsonModel { statusCode = 300, message = "删除失败,记录不存在！", closeCurrent = false };
            if (string.IsNullOrEmpty(ids))
            {
                return Json(result);
            }
            var list = await StockRepository.BatchDeleteAsync(ids);
            if (list == null)
            {
                return Json(result);
            }
            var msg = string.Join(",", list.Select(m => m.Name).ToArray()).ToEllipsis(100);
            await LogRepository.Insert(TableSource.Stock, OperationType.Delete, string.Format("批量删除{0}等股票", msg), ids);
            result.statusCode = 200;
            result.message = "删除成功！";
            return Json(result);
        }
        #endregion
    }
}