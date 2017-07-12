﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
﻿using PwC.C4.Configuration.Data;
﻿using PwC.C4.DataService.Helpers;
﻿using PwC.C4.DataService.Model;
﻿using PwC.C4.DataService.Model.Enum;
﻿using PwC.C4.Infrastructure.BaseLogger;
﻿using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Data;
﻿using PwC.C4.Infrastructure.Data.MapperDelegates;
﻿using PwC.C4.Infrastructure.Helper;

namespace PwC.C4.DataService.Persistance
{
    internal static class InvitationDao
    {
        private static readonly LogWrapper Log = new LogWrapper();
        public static string AddInvitation(DataTable invitation, DataTable invitationRoles)
        {
            try
            {
                var identityid = "";
                var db = Database.GetDatabase(DatabaseInstance.C4Base);
                var count = SafeProcedure.ExecuteNonQuery(db,
                    "dbo.Invitaion_Insert"
                    , parameters =>
                    {
                        parameters.AddWithValue("@invitaion", invitation);
                        parameters.AddWithValue("@invitaion_roles", invitationRoles);
                        parameters.AddTypedDbNull("@num", ParameterDirectionWrap.Output, DbType.Guid);
                    }, outputparameters =>
                    {
                        identityid = outputparameters.GetValue("@num").ToString();
                    });
                return count > 0 ? identityid : "None";
            }
            catch (Exception ee)
            {
                Log.Error("InsertToMailQueue error,data:" + JsonHelper.Serialize(invitation), ee);
                return "Error";
            }
        }
    } 
}
