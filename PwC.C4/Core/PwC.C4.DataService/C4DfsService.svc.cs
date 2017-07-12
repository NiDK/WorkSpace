using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using PwC.C4.DataService.Model;
using PwC.C4.DataService.Model.Enum;
using PwC.C4.DataService.Persistance;

namespace PwC.C4.DataService
{
    public class C4DfsService : IC4DfsService
    {
        public Dictionary<string, object> RetrieveRecord(string appCode, string fileId, IList<string> properties)
        {
            return DfsDao.RetrieveRecord(appCode, fileId, properties);
        }

        public bool StartInsert(string appCode, string fileId,string uploader, Dictionary<string, object> properties)
        {
            return DfsDao.StartInsert(appCode, fileId, uploader, properties);
        }

        public void FinishInsert(string appCode, string fileId,string dfsPath)
        {
             DfsDao.FinishInsert(appCode, fileId, dfsPath);
        }


        public void RemoveRecord(string appCode, string fileId)
        {
            DfsDao.RemoveRecord(appCode, fileId);
        }

        public List<Dictionary<string, object>> GetDataRecords(string appCode, int pageIndex, int pageSize, string keyword, out long totalCount, string keyspace = null, string staffId = null)
        {
            return DfsDao.GetDataRecords(appCode, pageIndex, pageSize, keyword, out totalCount,keyspace, staffId);
        }

        public string GetDfsPathById(string appCode, string fileId)
        {
            return DfsDao.GetDfsPathById(appCode, fileId);
        }

        public string GetDfsPathBySize(string appCode, string fileId, string size)
        {
            return DfsDao.GetDfsPathBySize(appCode, fileId, size);
        }
    }
}
