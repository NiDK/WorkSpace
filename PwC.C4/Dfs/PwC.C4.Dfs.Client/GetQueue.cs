using System;
using Microsoft.Ccr.Core;
using PwC.C4.Dfs.Client.Helper;
using PwC.C4.Dfs.Common.Config;
using PwC.C4.Dfs.Common.Model;
using PwC.C4.Infrastructure.Logger;

namespace PwC.C4.Dfs.Client
{
    internal class GetQueue
    {
        #region Fields

        private static readonly LogWrapper _logger = new LogWrapper();

        private Dispatcher _dispatcher;
        private DispatcherQueue _queue;
        private Port<RetrieveParam> _port;

        private Port<RetrieveParam> Port
        {
            get
            {
                if (_port == null)
                {
                    lock (this)
                    {
                        if (_port == null)
                        {
                            BuildQueue();
                            Arbiter.Activate(_queue, Arbiter.Receive<RetrieveParam>(true, _port, RetrieveHandler));
                        }
                    }
                }

                return _port;
            }
        }

        #endregion

        #region Helper

        private void BuildQueue()
        {
            var config = DfsConfig.Instance.GetQueueConfig;

            _dispatcher = new Dispatcher(config.ThreadCount, config.ThreadPriority, config.UseBackgroundThreads, "DfsGetDispatcher");
            _queue = new DispatcherQueue("DfsGetQueue", _dispatcher, config.ThrottlePolicy, config.MaxQueueDepth);
            _port = new Port<RetrieveParam>();
        }

        private static void RetrieveHandler(RetrieveParam param)
        {
            try
            {
                DfsItem item;
                param.Result = GetDfsItem(param.DfsPath, out item);
                param.ItemHandler(param.Result, item);
            }
            finally
            {
                param.Counter.Decrement();
            }
        }

        private static DfsOperationResult GetDfsItem(string path, out DfsItem item)
        {
            DfsOperationResult result;

            try
            {
                item = Dfs.Get(path);
                result = item != null ? DfsOperationResult.Success(path)
                                      : DfsOperationResult.Fail(path, "File Not Found");
            }
            catch (Exception ex)
            {
                _logger.HandleException(ex, "DfsClient");

                item = null;
                result = DfsOperationResult.Fail(path, ex.Message);
            }

            return result;
        }

        #endregion

        public DfsOperationResult[] Get(string[] paths, Dfs.DfsItemGetCallback itemHandler, int milliseconds, out bool timeout)
        {
            var results = new DfsOperationResult[paths.Length];

            var counter = new ZeroActionCounter(paths.Length);

            for (int i = 0; i < paths.Length; ++i)
            {
                Port.Post(new RetrieveParam(paths[i], itemHandler, counter, results, i));
            }

            // wait for completion
            timeout = !counter.WaitForZero(milliseconds);

            return results;
        }

        private class RetrieveParam
        {
            public string DfsPath { get; private set; }
            public Dfs.DfsItemGetCallback ItemHandler { get; private set; }
            public ZeroActionCounter Counter { get; private set; }

            private DfsOperationResult[] results;
            private int resultIndex;

            public DfsOperationResult Result
            {
                get { return results[resultIndex]; }
                set { results[resultIndex] = value; }
            }

            public RetrieveParam(string path, Dfs.DfsItemGetCallback itemHandler, ZeroActionCounter counter,
                DfsOperationResult[] results, int resultIndex)
            {
                this.DfsPath = path;
                this.ItemHandler = itemHandler;
                this.Counter = counter;
                this.results = results;
                this.resultIndex = resultIndex;
            }
        }
    }
}
