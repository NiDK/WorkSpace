using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PwC.C4.ConnectionPool.Config;
using PwC.C4.ConnectionPool.Exceptions;

namespace PwC.C4.ConnectionPool
{
    internal class Service<T> : IService
        where T : XConnection
    {
        private ServiceDescription _description;
        private IDictionary<string, ICluster> _clusters;

        public Service(ServiceDescription description)
        {
            _description = description;
            _clusters = BuildClusterMap(description);
        }

        private IDictionary<string, ICluster> BuildClusterMap(ServiceDescription description)
        {
            var map = new Dictionary<string, ICluster>(description.Clusters.Count, StringComparer.OrdinalIgnoreCase);

            foreach (var cluster in description.Clusters)
            {
                map[cluster.Name] = new Cluster<T>(cluster);
            }

            return map;
        }

        public ICluster GetCluster(string clusterName)
        {
            ICluster cluster;
            if (_clusters.TryGetValue(clusterName, out cluster))
            {
                return cluster;
            }

            throw new ConnectionPoolException(string.Format("{0}:{1} not found", _description.Name, clusterName));
        }

        public void ErrorCounterTick()
        {
            foreach (var cluster in _clusters.Values)
            {
                cluster.ErrorCounterTick();
            }
        }

        public void Reload(ServiceDescription description)
        {
            _description = description;

            var current = _clusters;
            var next = RebuildClusterMap(current, description);
            _clusters = next;

            DisposeRemovedClusters(current, next);
        }

        private void DisposeRemovedClusters(IDictionary<string, ICluster> oldMap, IDictionary<string, ICluster> newMap)
        {
            foreach (var kvp in oldMap)
            {
                if (!newMap.ContainsKey(kvp.Key))
                {
                    kvp.Value.Dispose();
                }
            }
        }

        private IDictionary<string, ICluster> RebuildClusterMap(IDictionary<string, ICluster> oldMap, ServiceDescription newDescription)
        {
            var next = new Dictionary<string, ICluster>(newDescription.Clusters.Count, StringComparer.OrdinalIgnoreCase);

            foreach (var clusterDescription in newDescription.Clusters)
            {
                ICluster cluster;
                if (oldMap.TryGetValue(clusterDescription.Name, out cluster))
                {
                    cluster.Reload(clusterDescription);

                    next[clusterDescription.Name] = cluster;
                    continue;
                }

                next[clusterDescription.Name] = new Cluster<T>(clusterDescription);
            }

            return next;
        }
    }
}
