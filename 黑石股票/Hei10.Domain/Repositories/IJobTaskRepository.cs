using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Enums;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;
using PagedList;

namespace Hei10.Domain.Repositories
{
    public interface IJobTaskRepository : IRepository<JobTask>
    {

        /// <summary>
        /// 返回所有的记录
        /// </summary>
        /// <param name="name">搜索关键词</param>
        /// <param name="status"></param>
        /// <param name="orderDirection"></param>
        /// <param name="pageCurrent">当前页</param>
        /// <param name="pageSize">每页显示总数</param>
        /// <param name="orderField"></param>
        /// <returns></returns>
        IPagedList<JobTask> GetPagedList(string name, CommonStatus? status, string orderField, string orderDirection, int pageCurrent, int pageSize);

        List<JobTask> GetList();

        JobTask GetByTaskId(string taskId);
    }
}
