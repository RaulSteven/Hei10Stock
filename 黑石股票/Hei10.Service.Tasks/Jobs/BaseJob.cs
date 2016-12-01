﻿using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using log4net;
using Hei10.Domain.Repositories;
using Hei10.Service.Tasks.Infrastructure;
using Quartz;

namespace Hei10.Service.Tasks.Jobs
{
    public abstract class BaseJob : IJob
    {
        private static readonly object LockObj = new object();

        public ILog Log { get; private set; }

        protected BaseJob()
        {
            Log = LogManager.GetLogger(this.GetType().FullName);
        }

        public void Execute(IJobExecutionContext context)
        {
            lock (LockObj)
            {
                try
                {
                    Log.Info("开始任务!");
                    //DependencyConfig.Register();
                   ExcuteTask();
                    Log.Info("结束任务!");
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    //SendEmail(ex);
                }
            }
        }

        public virtual void ExcuteTask() {
        }
    }
}