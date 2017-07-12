using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PwC.C4.Infrastructure.Logger;
using Quartz;
using Quartz.Impl;

namespace PwC.C4.Testing.Scheduler.ServiceConsole
{
    class Program
    {
        static readonly LogWrapper Log = new LogWrapper();

        static void Main(string[] args)
        {
            try
            {
                var enablePlugs = PluginConfigsExtension.GetEnablePlugin();
                var scheduler = StdSchedulerFactory.GetDefaultScheduler();
                foreach (var pluginConfig in enablePlugs)
                {
                    var assemblyInfo = pluginConfig.AssemblyInfo.Split(new string[] {","},
                        StringSplitOptions.RemoveEmptyEntries);
                    var assembly = Assembly.Load(assemblyInfo[0]);
                    var type = assembly.GetTypes().FirstOrDefault(t => t.FullName == assemblyInfo[1]);
                    var job = JobBuilder.Create(type).SetJobData(new JobDataMap(pluginConfig.ParameterDic)).Build();
                    var sc = CronScheduleBuilder.CronSchedule(pluginConfig.CornExpression);
                    var trigger = TriggerBuilder.Create().WithSchedule(sc).StartNow().Build();
                    scheduler.ScheduleJob(job, trigger);
                }
                scheduler.Start();
            }
            catch (Exception ee)
            {
                Log.Error("Scheduler error!", ee);
            }

        }
    }
}
