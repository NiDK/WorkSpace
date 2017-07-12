using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PwC.C4.ConnectionPool.Config;
using PwC.C4.ConnectionPool.Exceptions;

namespace PwC.C4.ConnectionPool
{
    public class XConnectionManager<T> where T : XConnection
    {
        private IService _service;
        private Timer _timer;

        public XConnectionManager(ServiceDescription description)
        {
            _service = new Service<T>(description);
            _timer = new Timer(ErrorCounterTick, null, 500, 500);
        }

        public T GetConnection(string clusterName)
        {
            var cluster = _service.GetCluster(clusterName);
            var node = cluster.GetNode();
            return node.GetConnection() as T;
        }

        public T GetConnection(string clusterName, string preferredNode)
        {
            var cluster = _service.GetCluster(clusterName);
            var node = cluster.GetNode(preferredNode);
            return node.GetConnection() as T;
        }

        private void ErrorCounterTick(object state)
        {
            _service.ErrorCounterTick();
        }

        public void Reload(ServiceDescription description)
        {
            lock (this)
            {
                _service.Reload(description);
            }
        }
    }
}
