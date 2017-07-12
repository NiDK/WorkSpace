using System;
using PwC.C4.Dfs.Common.Model;

namespace PwC.C4.Dfs.Client
{
    public partial class Dfs
    {
        #region Store

        private delegate DfsPath StoreDelegate(DfsItem item,string staffId);
        private static readonly StoreDelegate StoreHandler = Store;

        public static IAsyncResult BeginStore(DfsItem item, string staffId, AsyncCallback callback, object state)
        {
            return StoreHandler.BeginInvoke(item, staffId, callback, state);
        }

        public static DfsPath EndStore(IAsyncResult asyncResult)
        {
            return StoreHandler.EndInvoke(asyncResult);
        }

        #endregion

        #region Multi Store

        private delegate DfsOperationResult[] MultiStoreDelegate(DfsItem[] items,string staffId);
        private static readonly MultiStoreDelegate MultiStoreHandler = Store;

        public static IAsyncResult BeginMultiStore(DfsItem[] items, string staffId, AsyncCallback callback, object state)
        {
            return MultiStoreHandler.BeginInvoke(items, staffId,callback, state);
        }

        public static DfsOperationResult[] EndMultiStore(IAsyncResult asyncResult)
        {
            return MultiStoreHandler.EndInvoke(asyncResult);
        }

        #endregion

        #region Timed Multi Store

        private delegate DfsOperationResult[] TimedMultiStoreDelegate(DfsItem[] items, string staffId, int milliseconds, out bool timeout);
        private static readonly TimedMultiStoreDelegate TimedMultiStoreHandler = Store;

        public static IAsyncResult BeginMultiStore(DfsItem[] items, string staffId, int milliseconds, out bool timeout,
                                                   AsyncCallback callback, object state)
        {
            return TimedMultiStoreHandler.BeginInvoke(items, staffId, milliseconds, out timeout, callback, state);
        }

        public static DfsOperationResult[] EndMultiStore(out bool timeout, IAsyncResult asyncResult)
        {
            return TimedMultiStoreHandler.EndInvoke(out timeout, asyncResult);
        }

        #endregion

        #region Multi Get

        private delegate DfsOperationResult[] MultiGetDelegate(string[] paths, DfsItemGetCallback itemHandler);
        private static readonly MultiGetDelegate MultiGetHandler = Get;

        public static IAsyncResult BeginMultiGet(string[] paths, DfsItemGetCallback itemHandler, AsyncCallback callback, object state)
        {
            return MultiGetHandler.BeginInvoke(paths, itemHandler, callback, state);
        }

        public static DfsOperationResult[] EndMultiGet(IAsyncResult asyncResult)
        {
            return MultiGetHandler.EndInvoke(asyncResult);
        }

        #endregion

        #region Timed Multi Get

        private delegate DfsOperationResult[] TimedMultiGetDelegate(string[] paths, DfsItemGetCallback itemHandler, int milliseconds, out bool timeout);
        private static TimedMultiGetDelegate TimedMultiGetHandler = Get;

        public static IAsyncResult BeginMultiGet(string[] paths, DfsItemGetCallback itemHandler, int milliseconds, out bool timeout,
                                                 AsyncCallback callback, object state)
        {
            return TimedMultiGetHandler.BeginInvoke(paths, itemHandler, milliseconds, out timeout, callback, state);
        }

        public static DfsOperationResult[] EndMultiGet(out bool timeout, IAsyncResult asyncResult)
        {
            return TimedMultiGetHandler.EndInvoke(out timeout, asyncResult);
        }

        #endregion
    }
}
