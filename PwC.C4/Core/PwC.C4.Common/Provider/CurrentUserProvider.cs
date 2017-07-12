using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ApplicationCenter.ClientClass;
using ApplicationCenter.Model;
using PwC.C4.Common.Model.Const;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Infrastructure.Logger;
using MenuRole = ApplicationCenter.Model.vwMenuRole;

namespace PwC.C4.Common.Provider
{
    public static class CurrentUserProvider
    {

        static readonly string Mode = AppSettings.Instance.GetAuthenticateMode();

        static readonly LogWrapper Log = new LogWrapper();
        /// <summary>
        /// Gets user name
        /// </summary>
        [Obsolete("Please to use PwC.C4.Membership.CurrentUser.StaffName")]
        public static string StaffName
        {
            get
            {
                try
                {
                    var appcode = AppSettings.Instance.GetAppCode();
                    switch (Mode.ToLower())
                    {
                        case "pc":
                            return HttpContext.Current.Items["UserName"].ToString();
                        case "mobile":
                            var webuser = new WebUser("BasicHttpBinding_IApplicationService");
                            if (webuser.CheckAuthencatedForMobileWebApp(appcode))
                            {
                                var user = webuser.GetCurrentUserForMobileWebApp();
                                return user.UserName;
                            }
                            return "No current user";
                        case "both":
                            var userName = HttpContext.Current.Items["UserName"];
                            if (!string.IsNullOrEmpty(userName?.ToString()))
                            {
                                return userName.ToString();
                            }
                            //var webuserb = new WebUser("BasicHttpBinding_IApplicationService");
                            //if (webuserb.CheckAuthencatedForMobileWebApp(appcode))
                            //{
                            //    var user = webuserb.GetCurrentUserForMobileWebApp();
                            //    userName = user.UserName;
                            //}
                            //else
                            //{

                            //}
                            throw new Exception("AuthenticateMode can not use \"Both\" so far!");
                        default:
                            return "No current user";
                    }
                }
                catch (Exception ee)
                {
                    Log.Error("StaffName is Null,Mode:" + Mode, ee);
                    return "No current user";
                }

            }
        }

        /// <summary>
        /// Gets staff id
        /// </summary>
        [Obsolete("Please to use PwC.C4.Membership.CurrentUser.StaffId")]
        public static string StaffId
        {
            get
            {
                try
                {
                    var appcode = AppSettings.Instance.GetAppCode();
                    switch (Mode.ToLower())
                    {
                        case "pc":
                            return HttpContext.Current.User.Identity.Name;
                        case "mobile":

                            var webuser = new WebUser("BasicHttpBinding_IApplicationService");
                            if (webuser.CheckAuthencatedForMobileWebApp(appcode))
                            {
                                var user = webuser.GetCurrentUserForMobileWebApp();
                                return user.UserID;
                            }
                            else
                            {
                                Log.Error("CheckAuthencatedForMobileWebApp is false,appcode:" + appcode + ",webuser:" +
                                          JsonHelper.Serialize(webuser));
                            }
                            return "No current id";
                        case "both":
                            var userId = "No current id";
                            //var webuserb = new WebUser("BasicHttpBinding_IApplicationService");
                            //if (webuserb.CheckAuthencatedForMobileWebApp(appcode))
                            //{
                            //    var user = webuserb.GetCurrentUserForMobileWebApp();
                            //    userId = user.UserID;
                            //}
                            //else
                            //{
                            userId = HttpContext.Current.User.Identity.Name;
                            //}
                            throw new Exception("AuthenticateMode can not use \"Both\" so far!");
                        //return userId;
                        default:
                            return "No current id";
                    }
                }
                catch (Exception rr)
                {
                    Log.Error("StaffId is Null,Mode:" + Mode, rr);
                    return "No current id";
                }

            }
        }

        /// <summary>
        /// Gets current user roles type
        /// </summary>
        [Obsolete("Please to use PwC.C4.Membership.CurrentUser.Roles")]
        public static IEnumerable<string> CurrentRole
        {
            get
            {
                try
                {
                    var appcode = AppSettings.Instance.GetAppCode();
                    var user = new CurrentUser();
                    switch (Mode.ToLower())
                    {
                        case "pc":
                            var u = new WebUser();
                            var sid = HttpContext.Current.User.Identity.Name;
                            user = u.GetCurrentUser(sid, appcode);
                            break;
                        case "mobile":
                            var webuser = new WebUser("BasicHttpBinding_IApplicationService");
                            if (webuser.CheckAuthencatedForMobileWebApp(appcode))
                            {
                                user = webuser.GetCurrentUserForMobileWebApp();
                            }
                            break;
                        case "both":
                            throw new Exception("AuthenticateMode can not use \"Both\" so far!");
                        //var webuserb = new WebUser("BasicHttpBinding_IApplicationService");
                        //if (webuserb.CheckAuthencatedForMobileWebApp(appcode))
                        //{
                        //    user = webuserb.GetCurrentUserForMobileWebApp();
                        //}
                        //else
                        //{
                        //var ub = new WebUser();
                        //    var sidb = HttpContext.Current.User.Identity.Name;
                        //    user = ub.GetCurrentUser(sidb, appcode);
                        ////}
                        //break;
                        default:

                            return new List<string>();
                    }
                    return user.Roles.Select(c => c.RoleName).ToList();
                }
                catch (Exception ee)
                {
                    Log.Error("CurrentRole is Null", ee);
                    return new List<string>();
                }

            }
        }

        public static bool Admin()
        {
            return CurrentRole.Contains(UserKey.Admin);
        }
        [Obsolete("Please to use PwC.C4.Membership.CurrentUser.Menus")]
        public static List<MenuRole> MenuRoles()
        {
            try
            {
                var appcode = AppSettings.Instance.GetAppCode();
                var menuList = new List<MenuRole>();
                switch (Mode.ToLower())
                {
                    case "pc":
                        var webuserPc = new WebUser();
                        menuList = webuserPc.GetUserMenus().OrderBy(m => m.MenuOrder).ToList();
                        break;
                    case "mobile":

                        var webuser = new WebUser("BasicHttpBinding_IApplicationService");
                        if (webuser.CheckAuthencatedForMobileWebApp(appcode))
                        {
                            menuList = webuser.GetUserMenus().OrderBy(m => m.MenuOrder).ToList();
                        }
                        break;
                    case "both":
                        throw new Exception("AuthenticateMode can not use \"Both\" so far!");
                    //var webuserb = new WebUser("BasicHttpBinding_IApplicationService");
                    //if (webuserb.CheckAuthencatedForMobileWebApp(appcode))
                    //{
                    //    menuList = webuserb.GetUserMenus().OrderBy(m => m.MenuOrder).ToList();
                    //}
                    //else
                    //{
                    //var webuserPcb = new WebUser();
                    //    menuList = webuserPcb.GetUserMenus().OrderBy(m => m.MenuOrder).ToList();
                    ////}
                    //break;
                    default:
                        break;
                }
                return menuList;
            }
            catch (Exception ee)
            {
                Log.Error("MenuRoles is Null", ee);
                return new List<MenuRole>();
            }

        }



    }
}
