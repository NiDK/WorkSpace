using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using PwC.C4.Configuration.Data;
using PwC.C4.DataService.Model;
using PwC.C4.DataService.Model.Enum;
using PwC.C4.Infrastructure.BaseLogger;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Data;

namespace PwC.C4.DataService.Persistance
{
    internal static class SecurityDao
    {
        public static FunctionCheckResult CheckFunctionRight(FunctionCheck functionCheck)
        {
            var db = Database.GetDatabase(DatabaseInstance.C4Base);

            var myentity = SafeProcedure.ExecuteScalar(db, "dbo.Security_CheckFunctionRight",
                delegate(IParameterSet parameters)
                {
                    parameters.AddWithValue("@appCode", functionCheck.AppCode);
                    parameters.AddWithValue("@roleName", functionCheck.RoleName.ToStrIdTable());
                    parameters.AddWithValue("@Area", functionCheck.Area);
                    parameters.AddWithValue("@Controller", functionCheck.Controller);
                    parameters.AddWithValue("@Action", functionCheck.Action);
                    parameters.AddWithValue("@url", functionCheck.Url);
                });

            switch (myentity.ToString())
            {
                case "0":
                    return FunctionCheckResult.NoPermissions;
                case "-1":
                    return FunctionCheckResult.FunctionNotExist;
                case "-2":
                    return FunctionCheckResult.RoleNotExist;
                default:
                    return FunctionCheckResult.Permissioned;
            }
        }
    }
}
