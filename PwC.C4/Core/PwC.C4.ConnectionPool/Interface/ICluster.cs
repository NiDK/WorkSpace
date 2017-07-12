using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PwC.C4.ConnectionPool.Config;

namespace PwC.C4.ConnectionPool
{
    internal interface ICluster
    {
        INode GetNode();
        INode GetNode(string preferredNode);

        void ErrorCounterTick();
        
        void Reload(ClusterDescription description);
        
        void Dispose();
    }
}
