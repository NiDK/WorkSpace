using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ApplicationCenter.ClientClass;
using PwC.C4.Common.Provider;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Logger;
using PwC.C4.Sketch.Models.Const;

namespace PwC.C4.Sketch.Service.Web
{
    /// <summary>
    /// 
    /// </summary>
    [HandleError]
    public class C4Controller : System.Web.Mvc.Controller
    {

        #region OnAuthorization

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            var log = new LogWrapper();
            try
            {

                //For Anonymous Action
                if (filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof (AllowAnonymousAttribute),
                    false)
                    ||
                    filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof (HandleErrorAttribute), false)
                    || filterContext.ActionDescriptor.IsDefined(typeof (AllowAnonymousAttribute), false))
                {
                    return;
                }

                //For Child Action
                if (filterContext.ActionDescriptor.IsDefined(typeof (ChildActionOnlyAttribute), false))
                {
                    return;
                }
                var roles = CurrentUserProvider.CurrentRole.ToList();

                if (roles.Contains(RoleNames.Admin))
                {
                    return;
                }

                var hasPermission = false;

                if (
                    filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof (AllowAnonymousAttribute),
                        false) ||
                    filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof (C4AuthorizeAttribute), false) ||
                    filterContext.ActionDescriptor.IsDefined(typeof (C4AuthorizeAttribute), false))
                {
                    var filters =
                        filterContext.ActionDescriptor.ControllerDescriptor.GetFilterAttributes(false);
                    if (filters.Count() == 1 && (filters.First() is HandleErrorAttribute))
                    {
                        filters = filterContext.ActionDescriptor.GetFilterAttributes(false); //Get Action Attributes
                    }
                    foreach (var attr in filters)
                    {
                        if (attr is C4AuthorizeAttribute)
                        {
                            var auth = attr as C4AuthorizeAttribute;
                            var results = roles.Intersect(auth.RoleNames, StringComparer.OrdinalIgnoreCase).Any();
                            if (results)
                            {
                                hasPermission = true;
                                break;
                            }
                        }
                    }
                }

                if (!hasPermission)
                {

                    log.Error("Action Name:" + filterContext.ActionDescriptor.ActionName + ",Controller Name:" +
                              filterContext.ActionDescriptor.ControllerDescriptor.ControllerName +
                              ",Current User Has No Permission, StaffId:" + CurrentUserProvider.StaffId + ",StaffName:" +
                              CurrentUserProvider.StaffName);
                    filterContext.Result = Redirect(AppSettings.Instance.GetNoAuthorizePageUrl());
                }
            }
            catch (Exception ee)
            {
                log.Error("Action Name:" + filterContext.ActionDescriptor.ActionName + ",Controller Name:" +
                              filterContext.ActionDescriptor.ControllerDescriptor.ControllerName +
                              ",System Error, StaffId:" + CurrentUserProvider.StaffId + ",StaffName:" +
                              CurrentUserProvider.StaffName,ee);
                filterContext.Result = Redirect(AppSettings.Instance.GetNoAuthorizePageUrl());
            }
           
        }

        #endregion
    }
}
