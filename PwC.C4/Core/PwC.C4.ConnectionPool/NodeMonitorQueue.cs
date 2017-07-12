using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using PwC.C4.Infrastructure.Logger;

namespace PwC.C4.ConnectionPool
{
    internal class NodeMonitorQueue
    {
        private static readonly LogWrapper _logger = new LogWrapper();

        private readonly object _locker = new object();
        private ConcurrentDictionary<INode, INode> _nodes;
        private Thread _monitorThread;

        #region ctor

        private static readonly NodeMonitorQueue _instance = new NodeMonitorQueue();
        internal static NodeMonitorQueue Instance
        {
            get { return _instance; }
        }

        private NodeMonitorQueue()
        {
            _nodes = new ConcurrentDictionary<INode, INode>();

            _monitorThread = new Thread(Check);
            _monitorThread.IsBackground = true;
            _monitorThread.Start();
        }

        #endregion

        internal void Enqueue(INode node)
        {
            _nodes[node] = node;

            Monitor.Enter(_locker);
            Monitor.PulseAll(_locker);
            Monitor.Exit(_locker);
        }

        private void WaitForTask()
        {
            Monitor.Enter(_locker);
            while (_nodes.Count == 0)
            {
                Monitor.Wait(_locker);
            }
            Monitor.Exit(_locker);
        }

        private void Check()
        {
            while (true)
            {
                try
                {
                    WaitForTask();

                    var nodes = _nodes.Values.ToArray();
                    foreach (var node in nodes)
                    {
                        if (node.Disposed)
                        {
                            INode n;
                            _nodes.TryRemove(node, out n);
                            continue;
                        }

                        node.ClearConnections();

                        CheckNode(node);
                    }

                    Thread.Sleep(3000);
                }
                catch(Exception ex)
                {
                    _logger.Error("Exception in NodeMonitorQueue", ex);
                }
            }
        }

        private void CheckNode(INode node)
        {
            try
            {
                using (var connection = node.GetConnection())
                {
                }

                INode n;
                _nodes.TryRemove(node, out n);

                node.Failure = false;
            }
            catch (Exception ex)
            {
                _logger.Error("Exception in NodeMonitorQueue", ex);
            }
        }
    }
}
