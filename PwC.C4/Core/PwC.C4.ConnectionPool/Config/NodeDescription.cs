using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PwC.C4.ConnectionPool.Config
{
    public class NodeDescription
    {
        public string Host { get; set; }
        public int Port { get; set; }

        public int FailureThreshold { get; set; }
        public int FailureWindowSeconds { get; set; }

        public override string ToString()
        {
            return string.Format("{0}:{1}", Host, Port);
        }
    }
}
