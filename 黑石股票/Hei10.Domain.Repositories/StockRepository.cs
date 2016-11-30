using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models.Stock;

namespace Hei10.Domain.Repositories
{
    public class StockRepository : Repository<Stock>, IStockRepository
    {
    }
}
