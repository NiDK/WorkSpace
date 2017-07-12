using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Configuration;
using PwC.C4.Configuration.Data;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Exceptions;
using PwC.C4.Infrastructure.Logger;
using PwC.C4.Membership.Model.Enum;
using PwC.C4.Membership.Service;

namespace PwC.C4.Membership.Config
{

    public static class MembershipSettingsExtension
    {

        static Infrastructure.Logger.LogWrapper _log = new LogWrapper();

        public static AuthProviderSettings GetAuthProviderSettings(this MembershipSettings settings,
            AuthProvider authName)
        {
            if (MembershipSettings.NodesCache.Count == 0)
            {
                MembershipSettings.LoadCache(MembershipSettings.Instance);
            }

            AuthProviderSettings node;
            MembershipSettings.NodesCache.TryGetValue(AuthConst.AuthE2N[authName], out node);
            return node;

        }

    }
}
