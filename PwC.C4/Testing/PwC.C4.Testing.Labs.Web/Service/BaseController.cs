using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PwC.C4.Common.Provider;
using PwC.C4.Infrastructure.Cache;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Logger;
using PwC.C4.Membership;
using PwC.C4.Membership.Service;
using PwC.C4.Membership.WebExtension;
using PwC.C4.Testing.Labs.Web.Models.Const;

namespace PwC.C4.Testing.Labs.Web.Service
{
    public class BaseController : C4Controller
    {
        //public string Culture;

        public BaseController()
        {
        }

        public string GetCulture()
        {
            var c = "en";
            var key = string.Format("InspireCultrue-Setting-{0}", CurrentUser.StaffId);
            if (RouteData != null && RouteData.Values != null && RouteData.Values.ContainsKey("culture") && !string.IsNullOrEmpty(RouteData.Values["culture"].ToString()))
            {
                var fromRoute = RouteData.Values["culture"].ToString();

                if (fromRoute != "Home")
                {
                    fromRoute = string.IsNullOrEmpty(fromRoute) ? "en" : fromRoute;
                    c = string.IsNullOrEmpty(fromRoute) ? "en" : fromRoute;
                }
                else
                {
                    c = "en";
                }
                   
                Preference.Set(key, c);
            }
            else
            {
                var fromCache = Preference.Get(key);
                c = string.IsNullOrEmpty(fromCache) ? "en" : fromCache;
            }
            return c;
        }

    }
}
