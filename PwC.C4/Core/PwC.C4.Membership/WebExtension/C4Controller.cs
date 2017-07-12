using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PwC.C4.DataService.Model.Enum;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Logger;
using PwC.C4.Membership.Config;
using PwC.C4.Membership.Service;

namespace PwC.C4.Membership.WebExtension
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
            var userProvider = ProviderFactory.GetProvider<IUserProvider>(AppSettings.Instance.GetAppCode());
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

                var roles = CurrentUser.Roles.ToList();


                var hasPermission = false;

                if (
                    filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof (AllowAnonymousAttribute),
                        false) ||
                    filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof (C4SecurityAttribute), false) ||
                    filterContext.ActionDescriptor.IsDefined(typeof (C4SecurityAttribute), false))
                {
                    var rd = filterContext.RouteData.Values;
                    var dt = filterContext.RouteData.DataTokens;
                    var insecureArea = "";
                    if (dt.ContainsKey("areas") || dt.ContainsKey("area"))
                    {
                        insecureArea = dt.ContainsKey("area") ? dt["area"].ToString() : "";
                        insecureArea = (insecureArea == "" && dt.ContainsKey("areas")) ? dt["areas"].ToString() : insecureArea;
                    }

                    var insecureController = rd.ContainsKey("controller") ? rd["controller"].ToString() : ""; 
                    var insecureAction = rd.ContainsKey("action") ? rd["action"].ToString() : "";
                    var checkResult = MembershipService.Instance()
                        .FunctionCheck(insecureArea, insecureController, insecureAction, "", roles);
                    if (checkResult == FunctionCheckResult.Permissioned)
                        return;
                    else
                    {
                        log.Error("C4Security error!,FunctionCheckResutl is " + checkResult);
                    }
                }

                if (
                    filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof (AllowAnonymousAttribute),
                        false) ||
                    filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof (C4AuthorizeAttribute), false) ||
                    filterContext.ActionDescriptor.IsDefined(typeof (C4AuthorizeAttribute), false))
                {
                    var actionFilter = filterContext.ActionDescriptor.GetFilterAttributes(false).ToList();
                   
                    if (actionFilter.Count > 0)
                    {
                       var filterAttributes = (IList<FilterAttribute>) actionFilter ?? actionFilter.ToList();
                        if (
                            filterAttributes.OfType<C4AuthorizeAttribute>()
                                .Select(attr => attr as C4AuthorizeAttribute)
                                .Select(auth => roles.Intersect(auth.VisitRole, StringComparer.OrdinalIgnoreCase).Any())
                                .Any(results => results))
                        {
                            hasPermission = true;
                        }
                    }
                    else
                    {
                        var filters = filterContext.ActionDescriptor.ControllerDescriptor.GetFilterAttributes(false);
                        var attributes = filters as FilterAttribute[] ?? filters.ToArray();
                        var filterAttributes = filters as IList<FilterAttribute> ?? attributes.ToList();
                        if (filterAttributes.Count() == 1 && (filterAttributes.First() is HandleErrorAttribute))
                        {
                            filterAttributes = filterContext.ActionDescriptor.GetFilterAttributes(false).ToList();
                            //Get Action Attributes
                        }
                        if (
                            filterAttributes.OfType<C4AuthorizeAttribute>()
                                .Select(attr => attr as C4AuthorizeAttribute)
                                .Select(auth => roles.Intersect(auth.VisitRole, StringComparer.OrdinalIgnoreCase).Any())
                                .Any(results => results))
                        {
                            hasPermission = true;
                        }
                    }
                    
                }

                if (!hasPermission)
                {

                    log.Error("Action Name:" + filterContext.ActionDescriptor.ActionName + ",Controller Name:" +
                              filterContext.ActionDescriptor.ControllerDescriptor.ControllerName +
                              ",Current User Has No Permission, StaffId:" + userProvider.StaffId() + ",StaffName:" +
                              userProvider.StaffName());
                    filterContext.Result = Redirect(AppSettings.Instance.GetNoAuthorizePageUrl());
                }
            }
            catch (Exception ee)
            {
                log.Error("Action Name:" + filterContext.ActionDescriptor.ActionName + ",Controller Name:" +
                          filterContext.ActionDescriptor.ControllerDescriptor.ControllerName +
                          ",System Error, StaffId:" + userProvider.StaffId() + ",StaffName:" +
                          userProvider.StaffName(), ee);
                filterContext.Result = Redirect(AppSettings.Instance.GetNoAuthorizePageUrl());
            }

        }

        #endregion
    }
}
