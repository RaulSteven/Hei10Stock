using Hei10.Service.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Hei10.Service.WinServer
{
    public partial class WinServer : ServiceBase
    {
        public WinServer()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            TaskManager.Start();
        }

        protected override void OnStop()
        {
            TaskManager.Stop();
        }
    }
}
