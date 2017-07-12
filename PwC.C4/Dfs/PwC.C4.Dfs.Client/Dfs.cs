using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using PwC.C4.Dfs.Client.Client;
using PwC.C4.Dfs.Client.Helper;
using PwC.C4.Dfs.Common;
using PwC.C4.Dfs.Common.Config;
using PwC.C4.Dfs.Common.Exceptions;
using PwC.C4.Dfs.Common.Model;
using PwC.C4.Dfs.Common.Model.Enums;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Infrastructure.Logger;

namespace PwC.C4.Dfs.Client
{
    public partial class Dfs
    {
        #region Fields

        private static readonly LogWrapper _logger = new LogWrapper();

        private static readonly StoreQueue _storeQueue = new StoreQueue();
        private static readonly GetQueue _getQueue = new GetQueue();

        #endregion

        #region URL Translation

        public static string ToClientUrl(string dfsPath, UrlSignDomain domain, string staffId)
        {
            ArgumentHelper.AssertNotEmpty(dfsPath, "dfsPath");

            var path = DfsPath.Parse(dfsPath);
            var securityLevel = GetSecurityLevel(path.Keyspace, path.AppCode);
            return DfsUrlHelper.SignUrl(securityLevel, path.ToClientUrl(), domain, staffId);
        }



        public static string ToClientUrl(string dfsPath, string application)
        {
            ArgumentHelper.AssertNotEmpty(dfsPath, "dfsPath");
            ArgumentHelper.AssertNotEmpty(application, "application");

            var path = DfsPath.Parse(dfsPath);
            var securityLevel = GetSecurityLevel(path.Keyspace, path.AppCode);
            return DfsUrlHelper.SignUrl(securityLevel, path.ToClientUrl(), application);
        }

        public static string ToClientUrl(string dfsPath, string application, SecurityPolicy policy)
        {
            ArgumentHelper.AssertNotEmpty(dfsPath, "dfsPath");
            ArgumentHelper.AssertNotEmpty(application, "application");
            ArgumentHelper.AssertNotNull(policy, "policy");

            var path = DfsPath.Parse(dfsPath);

            var context = new SignatureContext
            {
                Timestamp = DateTimeUtility.CurrentTimestamp.ToString(),
                Url = path.ToClientUrl(),
                AppCode = application,
                SecurityPolicy = policy
            };

            return DfsUrlHelper.SignUrl(context);
        }

        public static string ToDownloadUrl(string dfsPath, UrlSignDomain domain, string staffId)
        {
            ArgumentHelper.AssertNotEmpty(dfsPath, "dfsPath");

            var path = DfsPath.Parse(dfsPath);
            var securityLevel = GetSecurityLevel(path.Keyspace, path.AppCode);
            return DfsUrlHelper.SignUrl(securityLevel, path.ToDownloadUrl(), domain, staffId);
        }

        public static string ToDownloadUrl(string dfsPath, string appCode)
        {
            ArgumentHelper.AssertNotEmpty(dfsPath, "dfsPath");
            ArgumentHelper.AssertNotEmpty(appCode, "appCode");

            var path = DfsPath.Parse(dfsPath);
            var securityLevel = GetSecurityLevel(path.Keyspace, path.AppCode);
            return DfsUrlHelper.SignUrl(securityLevel, path.ToDownloadUrl(), appCode);
        }

        public static string ToClientUrlForEmail(string dfsPath, UrlSignDomain domain, string email, string staffId)
        {
            ArgumentHelper.AssertNotEmpty(dfsPath, "dfsPath");
            ArgumentHelper.AssertNotEmpty(email, "email");

            var path = DfsPath.Parse(dfsPath);
            var securityLevel = GetSecurityLevel(path.Keyspace, path.AppCode);
            return DfsUrlHelper.SignUrl(securityLevel, path.ToClientUrl(), domain, email, staffId);
        }

        public static string ToDownloadUrlForEmail(string dfsPath, UrlSignDomain domain, string email, string staffId)
        {
            ArgumentHelper.AssertNotEmpty(dfsPath, "dfsPath");
            ArgumentHelper.AssertNotEmpty(email, "email");

            var path = DfsPath.Parse(dfsPath);
            var securityLevel = GetSecurityLevel(path.Keyspace, path.AppCode);
            return DfsUrlHelper.SignUrl(securityLevel, path.ToDownloadUrl(), domain, email, staffId);
        }

        #endregion

        #region Store

        /// <summary>
        /// Store a file to DFS
        /// </summary>
        /// <param name="item">
        /// Parameters designated in DfsItem:
        /// FileType: string, required, Keyspace is selected according to FileType, e.g. "Image"
        /// FileName: string, required, The name of the file, and extension name is required, e.g. "u.jpg"
        /// FileId:   string, optional, A unique string identifies the file in DFS, only chars in [A-Za-z0-9_] is allowed,
        ///           If not designated(null or empty), a Guid is generated as the file id
        /// FileData: Stream or byte[], required, Contents of the file to be stored
        /// AppCode: int, required, ID of the application owns the file
        /// </param>
        /// <param name="staffId"></param>
        /// <returns>DfsPath uniquely identifies the file</returns>
        public static DfsPath Store(DfsItem item, string staffId)
        {
            var configInst = DfsConfig.Instance;
            ArgumentHelper.AssertNotNull(item, "item");

            long timestamp = DateTime.UtcNow.Ticks;
            //Delete(item.Path, timestamp);

            timestamp += TimeSpan.TicksPerMillisecond;
            var fileId = DfsClientHelper.StartInsert(item, staffId, timestamp);

            #region Wcf transfer

            var filePath = Path.Combine(item.AppCode, item.Keyspace);

            using (FileRepositoryServiceClient c = new FileRepositoryServiceClient())
            {
                c.PutFile(new FileUploadMessage()
                {
                    FileName = fileId + "." + item.FileExtension,
                    DfsPath = filePath,
                    DataStream = item.FileDataStream
                });
            }

            #endregion


            var path = new DfsPath(item.Keyspace, item.AppCode, fileId, item.FileExtension);

            DfsClientHelper.FinishInsert(item, fileId, path.ToString());

            return path;
        }


        public static DfsOperationResult[] Store(DfsItem[] items, string staffId)
        {
            bool timeout;
            return Store(items, staffId, Timeout.Infinite, out timeout);
        }

        /// <summary>
        /// Batch DFS Store
        /// </summary>
        /// <param name="items">Items to store in DFS</param>
        /// <param name="milliseconds">The number of milliseconds to wait, or Timeout.Infinite(-1) to wait indefinitely</param>
        /// <param name="timeout">Indicate whether the operation is timeout or not</param>
        /// <returns>
        /// DfsOperationResult[] in the order of the supplied items
        /// DfsOperationResult.Succeeded indicates whether the item stores successfully or not
        /// If success, DfsOperationResult.DfsPath contains the unique dfs path to the stored item
        /// If fail, DfsOperationResult.ErrorMessage gives information about why the operation is failed
        /// </returns>
        public static DfsOperationResult[] Store(DfsItem[] items, string staffId, int milliseconds, out bool timeout)
        {
            ArgumentHelper.AssertNotNull(items, "items");
            ArgumentHelper.AssertNotNullOrEmpty(staffId, "Staff id");
            ArgumentHelper.AssertPositive(items.Length, "items.Length");
            ArgumentHelper.AssertValuesNotNull(items);

            CheckBatch(items.Length, DfsConfig.Instance.MaxBatchStoreSize);

            return _storeQueue.Store(items, staffId, milliseconds, out timeout);
        }

        #endregion

        #region Delete

        /// <summary>
        /// Delete file with the designated path in DFS
        /// </summary>
        /// <param name="path"></param>
        public static void Delete(string path)
        {
            ArgumentHelper.AssertNotEmpty(path, "path");
            Delete(DfsPath.Parse(path));
        }

        /// <summary>
        /// Delete file with the designated path in DFS
        /// </summary>
        /// <param name="dfsPath"></param>
        public static void Delete(DfsPath path)
        {
            ArgumentHelper.AssertNotNull(path, "path");
            Delete(path, DateTime.UtcNow.Ticks);
        }

        private static void Delete(DfsPath path, long timestamp)
        {
            ArgumentHelper.AssertNotNull(path, "path");

            DfsClientHelper.DeleteRecord(path, timestamp);
        }

        #endregion

        #region Get

        public static DfsItem Get(string path)
        {
            ArgumentHelper.AssertNotEmpty(path, "path");
            return Get(DfsPath.Parse(path));
        }

        public static DfsItem Get(DfsPath path)
        {
            ArgumentHelper.AssertNotNull(path, "path");
            var orId = Helper.DfsClientHelper.GetOriginalFileId(path.FileId);
            var record = DfsClientHelper.RetrieveRecord(path.Keyspace, path.AppCode, orId);

            if (record != null && record.AppCode == path.AppCode)
            {
                using (var c = new FileRepositoryServiceClient())
                {
                    var fileStream = c.GetFileByDfsPath(path);
                    return new DfsItem(path, record.Name, fileStream, record.Metadata, record.Encoding, record.Timestamp);
                }
            }

            return null;
        }

        /// <summary>
        /// Get file from dfs server by size
        /// </summary>
        /// <param name="path">DfsPath</param>
        /// <param name="fileSize">具体的尺寸类型参照当前App在DfsConvertConfig中所设置的参数名，"Small=s","Middle=m","Large=l". 默认转换如下：视频类型包含"s","l"两种尺寸，图片类型包含"s","m","l"三种类型 </param>
        /// <returns></returns>
        public static DfsItem Get(DfsPath path, string fileSize)
        {
            ArgumentHelper.AssertNotNull(path, "path");
            if (string.IsNullOrEmpty(fileSize))
            {
                return Get(path);
            }
            var record = DfsClientHelper.GetDfsPathBySize(path.FileId, fileSize);

            DfsPath newFileDfs;
            return DfsPath.TryParse(record, out newFileDfs) ? Get(newFileDfs) : null;
        }

        public static DfsItem Get(string path, string fileSize)
        {
            ArgumentHelper.AssertNotEmpty(path, "path");
            return Get(DfsPath.Parse(path), fileSize);
        }

        public delegate void DfsItemGetCallback(DfsOperationResult result, DfsItem item);

        public static DfsOperationResult[] Get(string[] paths, DfsItemGetCallback itemHandler)
        {
            bool timeout;
            return Get(paths, itemHandler, Timeout.Infinite, out timeout);
        }

        public static DfsOperationResult[] Get(string[] paths, DfsItemGetCallback itemHandler, int milliseconds,
            out bool timeout)
        {
            ArgumentHelper.AssertNotNull(paths, "paths");
            ArgumentHelper.AssertPositive(paths.Length, "paths.Length");
            ArgumentHelper.AssertValuesNotEmpty(paths);
            ArgumentHelper.AssertNotNull(itemHandler, "itemHandler");

            CheckBatch(paths.Length, DfsConfig.Instance.MaxBatchGetSize);

            return _getQueue.Get(paths, itemHandler, milliseconds, out timeout);
        }

        #endregion

        #region Helper

        private static SecurityLevel GetSecurityLevel(string keyspace, string appCode)
        {
            return DfsConfig.Instance.GetSecurityLevel(keyspace, appCode);
        }

        private static void CheckBatch(int batch, int max)
        {
            if (max > 0 && batch > max)
            {
                string error = string.Format("Too many items supplied: {0}, max allowed: {1}", batch, max);
                throw new DfsException(error);
            }
        }

        #endregion

        public static List<DfsRecord> GetDfsRecords(int pageIndex, int pageSize, string keyword, out long totalCount,
            string keyspace = null, string staffId = null)
        {
            return DfsClientHelper.GetDfsRecord(pageIndex, pageSize, keyword, out totalCount, keyspace, staffId);
        }

        public static string GetDfsPathByFileId(string fileId)
        {
            return DfsClientHelper.GetDfsPathByFileId(fileId);
        }
    }
}
