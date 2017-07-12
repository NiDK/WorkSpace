using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PwC.C4.ConnectionPool.Config;
using PwC.C4.ConnectionPool.Util;

namespace PwC.C4.ConnectionPool
{
    internal class Node<T> : INode
        where T : XConnection
    {
        protected NodeDescription _description;
        protected ConnectionPoolConfig _connectionPoolConfig;

        protected IConnectionPool _connectionPool;

        public string Host
        {
            get { return _description.Host; }
        }

        public int Port
        {
            get { return _description.Port; }
        }

        public Node(NodeDescription description, ConnectionPoolConfig connectionPoolConfig)
        {
            _description = description;
            _connectionPoolConfig = connectionPoolConfig;

            _connectionPool = BuildConnectionPool(description.Host, description.Port, connectionPoolConfig);
            _errorCounter = new CumulativeCounter(2 * description.FailureWindowSeconds);
        }

        private IConnectionPool BuildConnectionPool(string host, int port, ConnectionPoolConfig config)
        {
            ConnectionPoolBase<T> pool;

            var type = config.Type;
            if (type == ConnectionPoolType.Default)
            {
                pool = new ConnectionPool<T>(host, port, config);
            }
            else
            {
                pool = new ConcurrentConnectionPool<T>(host, port, config);
            }

            pool.Node = this;

            return pool;
        }

        public XConnection GetConnection()
        {
            return _connectionPool.GetConnection();
        }

        public void Reload(NodeDescription description, ConnectionPoolConfig connectionPoolConfig)
        {
            if (description.FailureWindowSeconds != _description.FailureWindowSeconds)
            {
                _errorCounter = new CumulativeCounter(2 * description.FailureWindowSeconds);
            }

            _description = description;

            RebuildConnectionPool(description.Host, description.Port, connectionPoolConfig);
        }

        private void RebuildConnectionPool(string host, int port, ConnectionPoolConfig config)
        {
            if (!config.Equals(_connectionPoolConfig))
            {
                _connectionPoolConfig = config;

                var connectionPool = _connectionPool;
                _connectionPool = BuildConnectionPool(host, port, config);
                connectionPool.Dispose();
            }
        }

        public void ClearConnections()
        {
            _connectionPool.ClearConnections();
        }

        private bool _disposed;
        public bool Disposed
        {
            get { return _disposed; }
        }

        public void Dispose()
        {
            _disposed = true;
            _connectionPool.Dispose();
        }

        #region Server status tracking

        protected CumulativeCounter _errorCounter;
        public void Exception(Exception ex)
        {
            _errorCounter.Increment();
        }

        private bool _failure;
        public bool Failure
        {
            get { return _failure; }
            set
            {
                if (!value)
                {
                    _errorCounter = new CumulativeCounter(2 * _description.FailureWindowSeconds);
                }
                _failure = value;
            }
        }

        public void ErrorCounterTick()
        {
            int count = _errorCounter.Tick();

            if (!Failure)
            {
                if (count >= _description.FailureThreshold)
                {
                    Failure = true;

                    NodeMonitorQueue.Instance.Enqueue(this);
                }
            }
        }

        #endregion

        public override string ToString()
        {
            return _description.ToString();
        }
    }
}
