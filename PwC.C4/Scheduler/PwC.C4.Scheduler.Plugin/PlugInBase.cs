using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace PwC.C4.Scheduler.Plugin
{
    public class PlugInBase:IJob
    {
        public PlugInBase(IDictionary parameters)
        {
            
        }

        public void Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
