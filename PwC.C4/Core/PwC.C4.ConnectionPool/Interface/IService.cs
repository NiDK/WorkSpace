using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PwC.C4.ConnectionPool.Config;

namespace PwC.C4.ConnectionPool
{
    internal interface IService
    {
        ICluster GetCluster(string clusterName);

        void ErrorCounterTick();
        
        void Reload(ServiceDescription description);
    }
}
