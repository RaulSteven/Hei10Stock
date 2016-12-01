using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models.Stock;
using System.Data.Entity;

namespace Hei10.Domain.Repositories
{
    public class StockRecordRepository : Repository<StockRecord>, IStockRecordRepository
    {
        public StockRecord GetToday(long stockId)
        {
            var todayDate = DateTime.Now.Date;
            var obj = QueryEnable()
                .FirstOrDefault(m => m.StockId == stockId
                    && m.CreateTime >= todayDate);
            return obj;
        }
    }
}
