using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PwC.C4.ConnectionPool.Config
{
    public class ServiceDescription
    {
        public string Name { get; set; }

        public IList<ClusterDescription> Clusters { get; set; }
    }
}
