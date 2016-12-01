using Hei10.Core.Utilities;
using Hei10.Domain.Models.Stock;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hei10.WebSite.Areas.Admin.Models
{
    public class StockModel
    {
        public long Id { get; set; }
        [DisplayName("股票名称")]
        [Required(ErrorMessage = ErrorMsgUtils.Required)]
        [MaxLength(50, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string Name { get; set; }

        [DisplayName("编号")]
        [Required(ErrorMessage = ErrorMsgUtils.Required)]
        [MaxLength(50, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string Code { get; set; }

        public long StockMarketId { get; set; }

        public List<StockMarket> MarketList { get; set; }
    }
}