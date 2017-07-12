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
    public class C4LogService : IC4LogService
    {
        public void Log_ForUserBehavior_Insert(string appcode, string optionType, string optionId, string method, string userId, string description)
        {
            LogDao.InsertUserBehaviorLog(appcode,optionType, optionId, method, userId, description);
        }

        public void Log_ForException_Insert(string appCode, string type, DateTime date, string staffId, string thread, string level,
            string message,
            string exp, string logger, int status)
        {
            LogDao.InsertExceptionLog(appCode, type,date, staffId, thread, level, message, exp, logger, status);
        }

        public void Log_FortMetadata_Insert(string appcode, string metadataobject, object dataId, MetadataLogType method, string json,
            string userId)
        {
            LogDao.InsertMetadataLog(appcode, metadataobject, dataId, method, json, userId);
        }
    }
}
