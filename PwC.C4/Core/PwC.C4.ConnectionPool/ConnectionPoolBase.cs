using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using PwC.C4.ConnectionPool.Config;
using PwC.C4.ConnectionPool.Exceptions;
using PwC.C4.Infrastructure.Logger;

namespace PwC.C4.ConnectionPool
{
    internal abstract class ConnectionPoolBase<T> : IConnectionPool
        where T : XConnection
    {
        private static readonly LogWrapper _logger = new LogWrapper();

        internal INode Node { get; set; }

        protected string _host;
        protected int _port;
        protected ConnectionPoolConfig _config;

        protected SemaphoreSlim _limiter;

        #region ctor

        public ConnectionPoolBase(string host, int port, ConnectionPoolConfig config)
        {
            _host = host;
            _port = port;
            _config = config;

            _limiter = new SemaphoreSlim(config.Size, config.Size);
        }

        #endregion

        public void EnterLimit()
        {
            if (!_limiter.Wait(_config.SocketConfig.ConnectTimeout))
            {
                throw new ConnectionPoolException("No connection acquired before timeout, connection pool size may be too small.");
            }
        }

        public void ReleaseLimit()
        {
            _limiter.Release();
        }

        public XConnection GetConnection()
        {
            EnterLimit();

            try
            {
                return Get();
            }
            catch
            {
                ReleaseLimit();
                throw;
            }
        }

        protected abstract XConnection Get();

        public void ReleaseConnection(XConnection connection)
        {
            try
            {
                if (connection.LastError != null)
                {
                    this.Node.Exception(connection.LastError);
                    CloseConnection(connection);
                    return;
                }

                if (connection.Expired)
                {
                    CloseConnection(connection);
                    return;
                }

                if (!connection.Alive)
                {
                    CloseConnection(connection);
                    return;
                }

                Release(connection);
            }
            finally
            {
                ReleaseLimit();
            }
        }

        protected abstract void Release(XConnection connection);

        public abstract void ClearConnections();

        public abstract void Dispose();

        protected XConnection CreateConnection()
        {
            try
            {
                var connection = (T)Activator.CreateInstance(typeof(T), _host, _port, _config.SocketConfig, _config.ConnectionLifeTimeMinutes);
                connection.Pool = this;

                connection.Open();

                return connection;
            }
            catch (Exception ex)
            {
                this.Node.Exception(ex);
                throw;
            }
        }

        protected void CloseConnection(XConnection connection)
        {
            try
            {
                connection.Close();
            }
            catch (Exception ex)
            {
                _logger.Error("Exception during connection close", ex);
            }
        }
    }
}
