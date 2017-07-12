﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
﻿using PwC.C4.Configuration.Data;
﻿using PwC.C4.DataService.Model;
﻿using PwC.C4.DataService.Model.Enum;
﻿using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Data;

namespace PwC.C4.DataService.Persistance
{
    public static class LogDao
    {

        internal static void InsertUserBehaviorLog(string appCode,string optionType, string optionId, string method, string userId, string description)
        {
            Database db = Database.GetDatabase(DatabaseInstance.C4Base);
            SafeProcedure.ExecuteNonQuery(db,
                "dbo.Log_UserBehavior_Insert"
                , parameters =>
                {
                    parameters.AddWithValue("@AppCode", appCode);
                    parameters.AddWithValue("@OptionType", optionType);
                    parameters.AddWithValue("@OptionId", optionId);
                    parameters.AddWithValue("@Method", method);
                    parameters.AddWithValue("@Description", description);
                    parameters.AddWithValue("@UserId", userId);
                    parameters.AddWithValue("@IpAddress", "");
                    parameters.AddWithValue("@MachineName", "");
                });
        }

        internal static void InsertExceptionLog(string appCode, string type, DateTime date, string staffId, string thread, string level, string message, string exp, string logger, int status)
        {
            Database db = Database.GetDatabase(DatabaseInstance.C4Base);
            SafeProcedure.ExecuteNonQuery(db,
                "dbo.Log_Exception_Insert"
                , parameters =>
                {
                    parameters.AddWithValue("@appCode", appCode);
                    parameters.AddWithValue("@type", type);
                    parameters.AddWithValue("@date", date);
                    parameters.AddWithValue("@staffId", staffId);
                    parameters.AddWithValue("@thread", thread);
                    parameters.AddWithValue("@level", level);
                    parameters.AddWithValue("@message", message);
                    parameters.AddWithValue("@exception", exp);
                    parameters.AddWithValue("@logger", logger);
                    parameters.AddWithValue("@status", status);
                });
        }


        internal static void InsertMetadataLog(string appcode, string metadataobject, object dataId, MetadataLogType method,
         string json,
         string userId)
        {
            Database db = Database.GetDatabase(DatabaseInstance.C4Base);
            SafeProcedure.ExecuteNonQuery(db,
                "dbo.Log_Metadata_Insert"
                , parameters =>
                {
                    parameters.AddWithValue("@AppCode", appcode);
                    parameters.AddWithValue("@Metadata", metadataobject);
                    parameters.AddWithValue("@DataId", dataId);
                    parameters.AddWithValue("@Method", (int)method);
                    parameters.AddWithValue("@JsonData", json);
                    parameters.AddWithValue("@UserId", userId);
                    parameters.AddWithValue("@IpAddress", "");
                    parameters.AddWithValue("@MachineName", "");
                });
        }
    }
}
