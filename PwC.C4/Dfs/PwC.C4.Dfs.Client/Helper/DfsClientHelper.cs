using System;
using System.Collections.Generic;
using PwC.C4.Dfs.Common.Exceptions;
using PwC.C4.Dfs.Common.Model;
using PwC.C4.Infrastructure.Config;

namespace PwC.C4.Dfs.Client.Helper
{
    internal static class DfsClientHelper
    {
        public static DfsRecord RetrieveRecord(string keyspace,string appCode, string fileId)
        {
            var client = new C4DfsServiceClient();
            var record = client.RetrieveRecord(appCode, fileId, null);

            if (!record.Empty())
            {
                var result = new DfsRecord(record);
                return result;
            }

            return null;
        }

        public static string GetOriginalFileId(string fileIdBySize)
        {
            var data = fileIdBySize.Split(new string[] {"-"}, StringSplitOptions.RemoveEmptyEntries);
            if (data.Length >= 1)
                return data[0];
            return fileIdBySize;
        }

        public static void DeleteRecord(DfsPath path, long timestamp)
        {
            try
            {
                var client = new C4DfsServiceClient();
                client.RemoveRecord(path.AppCode, path.FileId);
            }
            catch (Exception ex)
            {
                throw new DfsException(string.Format("Delete File Failed, Path: {0}", path), ex);
            }
        }

        public static string StartInsert(DfsItem item,string staffId, long timestamp)
        {
            var fileId = item.FileId;
            var record = new DfsRecord
            {
                AppCode = item.AppCode,
                Name = item.FileName,
                Encoding = item.Encoding,
                Length = item.Length,
                Keyspace = item.Keyspace,
                Metadata = item.Metadata
            };

            var client = new C4DfsServiceClient();
            var result = client.StartInsert(item.AppCode, fileId, staffId, record.Properties);
            return result ? fileId : "";
        }

        public static void FinishInsert(DfsItem item,string fileId,string dfsPath)
        {
            var client = new C4DfsServiceClient();
            client.FinishInsert(item.AppCode, fileId, dfsPath);
        }

        public static List<DfsRecord> GetDfsRecord(int pageIndex, int pageSize, string keyword, out long totalCount
            , string keyspace = null, string staffId = null)
        {
            var client = new C4DfsServiceClient();
            var appcode = AppSettings.Instance.GetAppCode();
            var items = client.GetDataRecords(appcode, pageIndex, pageSize, keyword, keyspace, staffId, out totalCount);
            var list = new List<DfsRecord>();
            items.ForEach(v => { list.Add(new DfsRecord(v)); });
            return list;
        }

        public static string GetDfsPathByFileId(string fileId)
        {
            var client = new C4DfsServiceClient();
            var appcode = AppSettings.Instance.GetAppCode();
            if (fileId.IndexOf("-", StringComparison.Ordinal) > -1)
            {
                fileId = fileId.Split('-')[0];
            }
            return client.GetDfsPathById(appcode, fileId);
        }

        public static string GetDfsPathBySize(string fileId,string fileSize)
        {
            var client = new C4DfsServiceClient();
            var appcode = AppSettings.Instance.GetAppCode();
            return client.GetDfsPathBySize(appcode, fileId,fileSize);
        }
    }
}
