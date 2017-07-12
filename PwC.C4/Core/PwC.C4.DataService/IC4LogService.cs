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
    public interface IC4LogService
    {
        #region Log

        [OperationContract]
        void Log_ForUserBehavior_Insert(string appcode, string optionType, string optionId, string method, string userId,
            string description);

        [OperationContract]
        void Log_ForException_Insert(string appCode, string type, DateTime date, string staffId, string thread, string level, string message, string exp, string logger, int status);

        [OperationContract]
        void Log_FortMetadata_Insert(string appcode, string metadataobject, object dataId, MetadataLogType method,string json,string userId);


        #endregion
    }


}
