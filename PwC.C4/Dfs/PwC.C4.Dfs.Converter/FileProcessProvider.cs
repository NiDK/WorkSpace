using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using PwC.C4.Dfs.Converter.Config;
using PwC.C4.Infrastructure.Logger;
using Quartz;
using Quartz.Impl;

namespace PwC.C4.Dfs.Converter
{
    public class FileProcessProvider
    {
        private readonly ServiceSetting _serviceSetting;
        readonly LogWrapper _log = new LogWrapper();
        public FileProcessProvider()
        {
            _serviceSetting = DfsConvertConfig.Instance.GetServiceSetting();
        }

        public void StartScheduler()
        {
            try
            {
                var interval = _serviceSetting.Interval;
                var scheduler = StdSchedulerFactory.GetDefaultScheduler();
                var job = JobBuilder.Create<ProcessConvert>().Build();
                var trigger = TriggerBuilder.Create()
                    .WithSimpleSchedule(m => m.WithIntervalInSeconds(interval).RepeatForever()).StartNow().Build();
                scheduler.Start();
                _log.Error("Service Start,interval:" + interval);
                scheduler.ScheduleJob(job, trigger);

            }
            catch (Exception ee)
            {
                _log.Error("StartScheduler error",ee);
            }
            
        }
    }
}
