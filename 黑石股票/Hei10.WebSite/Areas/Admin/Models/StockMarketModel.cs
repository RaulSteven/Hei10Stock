using Hei10.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hei10.WebSite.Areas.Admin.Models
{
    public class StockMarketModel
    {
        public long Id { get; set; }
        [DisplayName("市场名称")]
        [Required(ErrorMessage = ErrorMsgUtils.Required)]
        [MaxLength(20, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string Name { get; set; }

        [DisplayName("接口地址")]
        [Required(ErrorMessage = ErrorMsgUtils.Required)]
        [MaxLength(250, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string RequestUrl { get; set; }

        [DisplayName("说明")]
        [MaxLength(500, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string Remark { get; set; }

        [DisplayName("股票Key")]
        [Required(ErrorMessage = ErrorMsgUtils.Required)]
        [MaxLength(10, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string StockKey { get; set; }

        [DisplayName("开盘价Key")]
        [Required(ErrorMessage = ErrorMsgUtils.Required)]
        [MaxLength(20, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string OpenPriceKey { get; set; }

        [DisplayName("前收盘价Key")]
        [Required(ErrorMessage = ErrorMsgUtils.Required)]
        [MaxLength(20, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string FormPriceKey { get; set; }

        [DisplayName("最高价Key")]
        [Required(ErrorMessage = ErrorMsgUtils.Required)]
        [MaxLength(20, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string MaxPriceKey { get; set; }

        [DisplayName("最低价")]
        [Required(ErrorMessage = ErrorMsgUtils.Required)]
        [MaxLength(20, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string MinPriceKey { get; set; }
    }
}