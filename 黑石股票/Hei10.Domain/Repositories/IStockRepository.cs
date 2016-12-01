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
   public interface IStockRepository : IRepository<Stock>
    {
        IPagedList<Stock> GetList(string name, string orderField, string orderDirection, int pageSize, int pageIndex);

        Task<List<Stock>> GetListAsync(long marketId);
    }
}
