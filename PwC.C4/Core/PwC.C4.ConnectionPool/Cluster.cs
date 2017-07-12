using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using PwC.C4.ConnectionPool.Config;
using PwC.C4.ConnectionPool.Exceptions;

namespace PwC.C4.ConnectionPool
{
    internal class Cluster<T> : ICluster
        where T : XConnection
    {
        private ClusterDescription _description;
        private IList<INode> _nodes;

        public Cluster(ClusterDescription description)
        {
            _description = description;
            _nodes = BuildNodes(description);
        }

        private IList<INode> BuildNodes(ClusterDescription description)
        {
            var nodes = new List<INode>(description.Nodes.Count);

            foreach (var node in description.Nodes)
            {
                nodes.Add(new Node<T>(node, description.ConnectionPoolConfig));
            }

            return nodes;
        }

        private ThreadLocal<Random> _random = new ThreadLocal<Random>(() => new Random());
        public INode GetNode()
        {
            var nodes = _nodes;

            int index = _random.Value.Next(nodes.Count);
            for (int i = 0; i < nodes.Count; ++i)
            {
                var node = nodes[(index + i) % nodes.Count];
                if (!node.Failure)
                {
                    return node;
                }
            }

            throw new ConnectionPoolException("No available node for '" + _description.Name + "'");
        }

        public INode GetNode(string preferredNode)
        {
            var nodes = _nodes;

            foreach (var node in nodes)
            {
                if (Equals(node.Host, preferredNode))
                {
                    if (!node.Failure)
                    {
                        return node;
                    }

                    break;
                }
            }

            return GetNode();
        }

        private bool Equals(string host1, string host2)
        {
            if (host1.Length == host2.Length)
            {
                for (int i = host1.Length - 1; i >= 0; --i)
                {
                    if (host1[i] != host2[i])
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        public void ErrorCounterTick()
        {
            foreach (var node in _nodes)
            {
                node.ErrorCounterTick();
            }
        }

        public void Reload(ClusterDescription description)
        {
            _description = description;

            var current = _nodes;
            var next = RebuildNodes(current, description);
            _nodes = next;

            DisposeRemovedNodes(current, next);
        }

        private void DisposeRemovedNodes(IList<INode> oldNodes, IList<INode> newNodes)
        {
            var map = newNodes.ToDictionary(n => n.ToString());

            foreach (var node in oldNodes)
            {
                if (!map.ContainsKey(node.ToString()))
                {
                    node.Dispose();
                }
            }
        }

        private IList<INode> RebuildNodes(IList<INode> oldNodes, ClusterDescription newDescription)
        {
            var next = new List<INode>(newDescription.Nodes.Count);

            var map = oldNodes.ToDictionary(n => n.ToString());
            foreach (var nodeDescription in newDescription.Nodes)
            {
                INode node;
                if (map.TryGetValue(nodeDescription.ToString(), out node))
                {
                    node.Reload(nodeDescription, newDescription.ConnectionPoolConfig);

                    next.Add(node);
                    continue;
                }

                next.Add(new Node<T>(nodeDescription, newDescription.ConnectionPoolConfig));
            }

            return next;
        }

        private bool _disposed;
        public void Dispose()
        {
            _disposed = true;

            foreach (var node in _nodes)
            {
                node.Dispose();
            }
        }
    }
}
