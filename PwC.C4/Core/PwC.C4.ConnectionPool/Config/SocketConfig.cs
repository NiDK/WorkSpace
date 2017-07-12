using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PwC.C4.ConnectionPool.Config
{
    public class SocketConfig : IEquatable<SocketConfig>
    {
        public int ConnectTimeout { get; set; }

        public int SendTimeout { get; set; }

        public int ReceiveTimeout { get; set; }

        public int SendBufferSize { get; set; }

        public int ReceiveBufferSize { get; set; }

        public SocketConfig()
        {
            ConnectTimeout = 3000;
            SendTimeout = 10000;
            ReceiveTimeout = 10000;
            SendBufferSize = 8192;
            ReceiveBufferSize = 8192;
        }

        public bool Equals(SocketConfig other)
        {
            return ConnectTimeout == other.ConnectTimeout
                && SendTimeout == other.SendTimeout
                && ReceiveTimeout == other.ReceiveTimeout
                && SendBufferSize == other.SendBufferSize
                && ReceiveBufferSize == other.ReceiveBufferSize;
        }
    }
}
