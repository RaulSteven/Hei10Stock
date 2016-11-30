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
   public partial class Stock:AggregateRoot
    {
        [DisplayName("股票名称")]
        [Required(ErrorMessage = ErrorMsgUtils.Required)]
        [MaxLength(50, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string Name { get; set; }

        [DisplayName("编号")]
        [Required(ErrorMessage = ErrorMsgUtils.Required)]
        [MaxLength(50, ErrorMessage = ErrorMsgUtils.MaxStringLength)]
        public string Code { get; set; }

        public long StockMarketId { get; set; }

    }
}
