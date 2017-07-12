using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using PwC.C4.ConnectionPool.Config;
using PwC.C4.ConnectionPool.Exceptions;
using PwC.C4.Infrastructure.Logger;

namespace PwC.C4.ConnectionPool
{
    public abstract class XConnection : IConnection, IDisposable
    {
        private static readonly LogWrapper _logger = new LogWrapper();

        internal IConnectionPool Pool { get; set; }

        protected string _host;
        protected int _port;
        protected SocketConfig _config;

        protected DateTime _expiration;
        public virtual bool Expired
        {
            get { return DateTime.Now >= _expiration; }
        }

        public abstract bool Alive { get; }

        public Exception LastError { get; set; }

        #region ctor

        public XConnection(string host, int port, SocketConfig config, int lifeTimeMinutes)
        {
            _host = host;
            _port = port;
            _config = config;

            _expiration = DateTime.Now.AddMinutes(lifeTimeMinutes);
        }

        #endregion

        protected abstract void OnOpen();
        public void Open()
        {
            OnOpen();
        }

        protected abstract void OnClose();
        public void Close()
        {
            OnClose();
        }

        public void Dispose()
        {
            this.Pool.ReleaseConnection(this);
        }
    }
}
