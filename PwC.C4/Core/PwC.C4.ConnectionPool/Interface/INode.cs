using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PwC.C4.ConnectionPool.Config;

namespace PwC.C4.ConnectionPool
{
    internal interface INode
    {
        string Host { get; }
        int Port { get; }

        XConnection GetConnection();

        void Reload(NodeDescription description, ConnectionPoolConfig connectionPoolConfig);

        bool Failure { get; set; }
        void Exception(Exception ex);
        void ErrorCounterTick();
        void ClearConnections();

        bool Disposed { get; }
        void Dispose();
    }
}
