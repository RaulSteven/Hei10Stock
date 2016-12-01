using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hei10.Domain.Entityframework;
using Hei10.Domain.Infrastructure;
using Hei10.Domain.Models;
using System.Data.Entity;
using Hei10.Domain.Enums;
using PagedList;

namespace Hei10.Domain.Repositories
{
    public class JobTaskRepository : Repository<JobTask>, IJobTaskRepository
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
        public IPagedList<JobTask> GetPagedList(string name, CommonStatus? status, string orderField, string orderDirection, int pageCurrent, int pageSize)
        {
            var query = QueryUnDelete();
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(d => d.TaskName.Contains(name));
            }
            if (status.HasValue)
            {
                query = query.Where(m => m.CommonStatus == status.Value);
            }
            return ToPageList(query, orderField, orderDirection, m => m.CreateTime, pageCurrent, pageSize);
        }

        public List<JobTask> GetList()
        {
            return QueryAll().OrderBy(m => m.CreateTime).ThenByDescending(m => m.Id).ToList();
        }

        public JobTask GetByTaskId(string taskId)
        {
            Guid guid;
            if (!Guid.TryParse(taskId, out guid)) return null;
            return QueryUnDelete().FirstOrDefault(m => m.TaskId == guid);
        }
    }
}
