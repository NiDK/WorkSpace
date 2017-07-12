using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PwC.C4.ConnectionPool.Config
{
    public class ClusterDescription
    {
        public string Name { get; set; }

        public IList<NodeDescription> Nodes { get; set; }

        public ConnectionPoolConfig ConnectionPoolConfig { get; set; }
    }
}
