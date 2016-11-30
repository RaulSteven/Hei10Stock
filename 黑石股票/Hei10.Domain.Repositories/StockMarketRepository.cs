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
    public class StockMarketRepository : Repository<StockMarket>, IStockMarketRepository
    {
        public Task<List<StockMarket>> GetListAsync()
        {
            var list = AdminQueryEnable()
                .ToListAsync();
            return list;
        }
    }
}
