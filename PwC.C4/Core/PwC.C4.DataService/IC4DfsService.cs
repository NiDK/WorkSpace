using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using PwC.C4.DataService.Model;
using PwC.C4.DataService.Model.Enum;

namespace PwC.C4.DataService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IC4DfsService
    {
        [OperationContract]
        Dictionary<string, object> RetrieveRecord(string appCode, string fileId, IList<string> properties);

        [OperationContract]
        bool StartInsert(string appCode, string fileId, string uploader, Dictionary<string, object> properties);

        [OperationContract]
        void FinishInsert(string appCode, string fileId,string dfsPath);

        [OperationContract]
        void RemoveRecord(string appCode, string fileId);

        [OperationContract]
        List<Dictionary<string, object>> GetDataRecords(string appCode, int pageIndex, int pageSize, string keyword,out long totalCount, string keyspace = null, string staffId = null);

        [OperationContract]
        string GetDfsPathById(string appCode, string fileId);

        [OperationContract]
        string GetDfsPathBySize(string appCode, string fileId,string size);
    }


}
