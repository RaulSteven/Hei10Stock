using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models.Stock;
using PagedList;

namespace Hei10.Domain.Repositories
{
    public class StockRepository : Repository<Stock>, IStockRepository
    {
        public IPagedList<Stock> GetList(string name, string orderField, string orderDirection, int pageSize, int pageIndex)
        {
            var query = AdminQueryEnable();
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(m => m.Name.Contains(name));
            }
            var list = ToPageList(query, orderField, orderDirection, m => m.Id, pageIndex, pageSize);
            return list;
        }
    }
}
