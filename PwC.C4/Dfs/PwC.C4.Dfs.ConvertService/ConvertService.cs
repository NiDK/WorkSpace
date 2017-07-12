using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using PwC.C4.Dfs.Converter;

namespace PwC.C4.Dfs.ConvertService
{
    public partial class ConvertService : ServiceBase
    {
        public ConvertService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var convertServer = new FileProcessProvider();
            convertServer.StartScheduler();
        }

        protected override void OnStop()
        {
        }
    }
}
