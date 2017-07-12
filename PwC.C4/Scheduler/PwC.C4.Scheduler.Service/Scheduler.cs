using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using PwC.C4.Infrastructure.Logger;
using Quartz;
using Quartz.Impl;

namespace PwC.C4.Scheduler.Service
{
    public partial class Scheduler : ServiceBase
    {
        readonly LogWrapper _log = new LogWrapper();
        public Scheduler()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                var enablePlugs = PluginConfigsExtension.GetEnablePlugin();
                var scheduler = StdSchedulerFactory.GetDefaultScheduler();
                foreach (var pluginConfig in enablePlugs)
                {
                    var assemblyInfo = pluginConfig.AssemblyInfo.Split(new string[] { "," },
                          StringSplitOptions.RemoveEmptyEntries);
                    var assembly = Assembly.Load(assemblyInfo[0]);
                    var type = assembly.GetTypes().FirstOrDefault(t => t.FullName == assemblyInfo[1]);
                    var job = JobBuilder.Create(type).Build();
                    var sc = CronScheduleBuilder.CronSchedule(pluginConfig.CornExpression);
                    var trigger = TriggerBuilder.Create().WithSchedule(sc).StartNow().Build();
                    scheduler.ScheduleJob(job, trigger);
                }
                scheduler.Start();
            }
            catch (Exception ee)
            {
                _log.Error("StartScheduler error", ee);
            }
        }

        protected override void OnStop()
        {
        }
    }
}
