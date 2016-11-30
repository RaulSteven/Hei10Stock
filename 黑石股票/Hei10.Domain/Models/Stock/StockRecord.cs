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
    public partial class StockRecord : AggregateRoot
    {
        public long StockId { get; set; }

        /// <summary>
        /// 开盘价
        /// </summary>
        public decimal OpenPrice { get; set; }

        /// <summary>
        /// 前收盘价
        /// </summary>
        public decimal FormPrice { get; set; }

        /// <summary>
        /// 最高价
        /// </summary>
        public decimal MaxPrice { get; set; }
        /// <summary>
        /// 最低价
        /// </summary>
        public decimal MinPrice { get; set; }

        public decimal MTR { get; set; }

        public decimal ATR { get; set; }
    }
}
