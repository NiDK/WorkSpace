using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using PwC.C4.ConnectionPool.Config;

namespace PwC.C4.ConnectionPool
{
    internal class ConcurrentConnectionPool<T> : ConnectionPoolBase<T>
        where T : XConnection
    {
        protected ConcurrentStack<XConnection> _connections;

        #region ctor

        public ConcurrentConnectionPool(string host, int port, ConnectionPoolConfig config)
            : base(host, port, config)
        {
            _connections = new ConcurrentStack<XConnection>();
        }

        #endregion

        protected override XConnection Get()
        {
            int timeout = 0;

            XConnection connection;
            while (_connections.TryPop(out connection))
            {
                if (!connection.Alive)
                {
                    CloseConnection(connection);
                    continue;
                }

                if (timeout == 0)
                {
                    if (connection.Expired)
                    {
                        ++timeout;
                        CloseConnection(connection);
                        continue;
                    }
                }

                return connection;
            }

            return CreateConnection();
        }

        protected override void Release(XConnection connection)
        {
            if (_disposed)
            {
                CloseConnection(connection);
            }
            else
            {
                _connections.Push(connection);
            }
        }

        public override void ClearConnections()
        {
            XConnection connection;
            while (_connections.TryPop(out connection))
            {
                CloseConnection(connection);
            }
        }

        protected bool _disposed;
        public override void Dispose()
        {
            _disposed = true;

            ClearConnections();
        }
    }
}
