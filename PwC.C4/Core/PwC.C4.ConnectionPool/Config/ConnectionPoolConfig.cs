using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PwC.C4.ConnectionPool.Config
{
    public enum ConnectionPoolType
    {
        Default,
        Concurrent
    }

    public class ConnectionPoolConfig : IEquatable<ConnectionPoolConfig>
    {
        public ConnectionPoolType Type { get; set; }
        public int Size { get; set; }
        public int ConnectionLifeTimeMinutes { get; set; }
        public SocketConfig SocketConfig { get; set; }

        public bool Equals(ConnectionPoolConfig other)
        {
            return Type == other.Type
                && Size == other.Size
                && ConnectionLifeTimeMinutes == other.ConnectionLifeTimeMinutes
                && SocketConfig.Equals(other.SocketConfig);
        }
    }
}
