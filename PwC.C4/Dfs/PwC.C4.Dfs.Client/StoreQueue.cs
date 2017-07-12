using System;
using Microsoft.Ccr.Core;

using PwC.C4.Dfs.Client.Helper;
using PwC.C4.Dfs.Common.Config;
using PwC.C4.Dfs.Common.Model;
using PwC.C4.Infrastructure.Logger;

namespace PwC.C4.Dfs.Client
{
    internal class StoreQueue
    {
        #region Fields

        private static readonly LogWrapper _logger = new LogWrapper();

        private Dispatcher _dispatcher;
        private DispatcherQueue _queue;
        private Port<StoreParam> _port;

        private Port<StoreParam> Port
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
                            Arbiter.Activate(_queue, Arbiter.Receive<StoreParam>(true, _port, StoreHandler));
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
            var config = DfsConfig.Instance.UploadQueueConfig;

            _dispatcher = new Dispatcher(config.ThreadCount, config.ThreadPriority, config.UseBackgroundThreads, "DfsStoreDispatcher");
            _queue = new DispatcherQueue("DfsStoreQueue", _dispatcher, config.ThrottlePolicy, config.MaxQueueDepth);
            _port = new Port<StoreParam>();
        }

        private DfsOperationResult StoreDfsItem(DfsItem item,string staffId)
        {
            try
            {
                var path = Dfs.Store(item, staffId);
                return DfsOperationResult.Success(path);
            }
            catch (Exception ex)
            {
                _logger.HandleException(ex, "DfsClient");
                return DfsOperationResult.Fail(ex.Message);
            }
        }

        private void StoreHandler(StoreParam param)
        {
            try
            {
                param.Result = StoreDfsItem(param.DfsItem,param.StaffId);
            }
            finally
            {
                param.Counter.Decrement();
            }
        }

        #endregion

        public DfsOperationResult[] Store(DfsItem[] items,string staffId, int milliseconds, out bool timeout)
        {
            var results = new DfsOperationResult[items.Length];

            var counter = new ZeroActionCounter(results.Length);

            for (int i = 0; i < items.Length; ++i)
            {
                Port.Post(new StoreParam(items[i], staffId, counter, results, i));
            }

            // wait for completion
            timeout = !counter.WaitForZero(milliseconds);

            return results;
        }

        private class StoreParam
        {
            public DfsItem DfsItem { get; private set; }
            public string StaffId { get; private set; }
            public ZeroActionCounter Counter { get; private set; }

            private DfsOperationResult[] results;
            private int resultIndex;

            public DfsOperationResult Result
            {
                get { return results[resultIndex]; }
                set { results[resultIndex] = value; }
            }

            public StoreParam(DfsItem dfsItem,string staffId, ZeroActionCounter counter, DfsOperationResult[] results, int resultIndex)
            {
                this.DfsItem = dfsItem;
                this.StaffId = staffId;
                this.Counter = counter;
                this.results = results;
                this.resultIndex = resultIndex;
            }
        }
    }
}
