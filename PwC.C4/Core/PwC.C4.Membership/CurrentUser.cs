using System;
using System.Collections.Generic;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Logger;
using PwC.C4.Membership.Model;
using PwC.C4.Membership.Service;

namespace PwC.C4.Membership
{
    public static class CurrentUser
    {
        private static readonly IUserProvider User =
            ProviderFactory.GetProvider<IUserProvider>(AppSettings.Instance.GetAppCode());

        private static readonly LogWrapper Log = new LogWrapper();

        private static bool _isDebugUser = AuthService.IsDebugUser();

        /// <summary>
        /// Gets User name
        /// </summary>
        public static string StaffName
        {
            get
            {
                try
                {
                    if (_isDebugUser)
                    {
                        return "Debug User";
                    }
                    return User.StaffName();
                }
                catch (Exception ee)
                {
                    Log.Error("StaffName is Null", ee);
                    return "No current User";
                }

            }
        }

        /// <summary>
        /// Gets staff id
        /// </summary>
        public static string StaffId
        {
            get
            {
                try
                {
                    if (_isDebugUser)
                    {
                        return "Debug User Id";
                    }
                    return User.StaffId();
                }
                catch (Exception rr)
                {
                    Log.Error("StaffId is Null", rr);
                    return "No current id";
                }

            }
        }

        public static string StaffCode
        {
            get
            {
                try
                {
                    if (_isDebugUser)
                    {
                        return "Debug User Code";
                    }
                    return User.StaffCode();
                }
                catch (Exception rr)
                {
                    Log.Error("StaffCode is Null", rr);
                    return "No StaffCode";
                }
            }
        }

        public static string OrganizationCode
        {
            get
            {
                try
                {
                    if (_isDebugUser)
                    {
                        return "Debug User OrganizationCode";
                    }
                    return User.OrganizationCode();
                }
                catch (Exception rr)
                {
                    Log.Error("OrganizationCode is Null", rr);
                    return "No OrganizationCode";
                }
            }
        }

        /// <summary>
        /// Gets current User roles type
        /// </summary>
        public static IEnumerable<string> Roles
        {
            get
            {
                try
                {
                    if (_isDebugUser)
                    {
                        return new List<string>() {"User","Admin","AppAdmin"};
                    }
                    return User.GetCurrentRoles();
                }
                catch (Exception ee)
                {
                    Log.Error("CurrentRole is Null", ee);
                    return new List<string>();
                }

            }
        }

        public static List<CurrentMenu> Menus
        {
            get
            {
                try
                {
                    if (_isDebugUser)
                    {
                        return new List<CurrentMenu>();
                    }
                    return User.GetCurrentMenu();
                }
                catch (Exception ee)
                {
                    Log.Error("MenuRoles is Null", ee);
                    return new List<CurrentMenu>();
                }
            }
        }

        public static bool IsMobile => User.IsMobileMode();

        public static CurrentUserInfo UseInfo
        {
            get
            {
                try
                {
                    if (_isDebugUser)
                    {
                        return new CurrentUserInfo()
                        {
                            StaffCode = "Debug user code",
                            StaffId = "Debug user Id",
                            StaffName = "Debug user",
                            Email = "DebugUser@Debug.com",
                            Phone = "10000001",
                            Roles = new List<string>() { "User","Admin","AppAdmin"},
                            Menus = new List<CurrentMenu>()
                        };
                    }
                    return User.GetCurrentUser();
                }
                catch (Exception ee)
                {
                    Log.Error("Current user is Null", ee);
                    return new CurrentUserInfo();
                }
            }
        }

    }
}
