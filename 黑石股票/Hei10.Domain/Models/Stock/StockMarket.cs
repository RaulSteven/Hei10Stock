using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Hei10.Core.Utilities;
namespace Hei10.Domain.Models.Stock
{
    public partial class StockMarket:AggregateRoot
    {
        [DisplayName("市场名称")]
        [Required(ErrorMessage =ErrorMsgUtils.Required)]
        [MaxLength(20,ErrorMessage =ErrorMsgUtils.MaxStringLength)]
        public string Name { get; set; }

        [DisplayName("接口地址")]
        [Required(ErrorMessage = ErrorMsgUtils.Required)]
        [MaxLength(250, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string RequestUrl { get; set; }

        [DisplayName("说明")]
        [MaxLength(500, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string Remark { get; set; }
    }
}
