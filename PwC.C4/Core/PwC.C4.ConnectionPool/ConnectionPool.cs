using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using PwC.C4.ConnectionPool.Config;

namespace PwC.C4.ConnectionPool
{
    internal class ConnectionPool<T> : ConnectionPoolBase<T>
        where T : XConnection
    {
        protected object _locker;
        protected Stack<XConnection> _connections;

        #region ctor

        public ConnectionPool(string host, int port, ConnectionPoolConfig config)
            : base(host, port, config)
        {
            _locker = new object();
            _connections = new Stack<XConnection>();
        }

        #endregion

        protected override XConnection Get()
        {
            lock (_locker)
            {
                int timeout = 0;

                while (_connections.Count > 0)
                {
                    var connection = _connections.Pop();
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
            }

            return CreateConnection();
        }

        protected override void Release(XConnection connection)
        {
            lock (_locker)
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
        }

        public override void ClearConnections()
        {
            lock (_locker)
            {
                while (_connections.Count > 0)
                {
                    CloseConnection(_connections.Pop());
                }
            }
        }

        protected bool _disposed;
        public override void Dispose()
        {
            lock (_locker)
            {
                _disposed = true;

                while (_connections.Count > 0)
                {
                    CloseConnection(_connections.Pop());
                }
            }
        }
    }
}
