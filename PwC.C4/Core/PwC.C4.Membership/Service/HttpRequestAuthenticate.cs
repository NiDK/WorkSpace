using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Infrastructure.Logger;
using PwC.C4.Membership.Model;
using PwC.C4.Membership.Model.Enum;

namespace PwC.C4.Membership.Service
{
    public class HttpRequestAuthenticate
    {
        static readonly LogWrapper Log = new LogWrapper();
        readonly HttpContext _context;
        readonly AuthProvider _authProvider;
        readonly string _appCode = null;
        readonly string _token;
        readonly string _serviceTicket;
        private readonly IUserProvider _provider;

        public HttpRequestAuthenticate(HttpContext context)
        {
            _context = context;
            _appCode = AppSettings.Instance.GetAppCode();
            _provider = ProviderFactory.GetProvider<IUserProvider>(_appCode);
            _authProvider = _provider.CurrentProvider();
            var httpCookie = _context.Request.Cookies[AuthConst.TokenInCookie(_provider.CurrentProvider())];
            if (httpCookie != null)
            {
                _token = httpCookie.Value;
            }
            else
            {
                _token = _context.Request.ServerVariables[AuthConst.TokenName];
                if (_token != null)
                {
                    var tokenincookie = new HttpCookie(AuthConst.TokenInCookie(_provider.CurrentProvider()), _token)
                    {
                        Expires = DateTime.Now.AddYears(10)
                    };
                    context.Response.Cookies.Add(tokenincookie);
                }
            } 
            
            _serviceTicket = _context.Request.QueryString[AuthConst.ServiceTicket];
        }

        public void Authenticate()
        {
            if (CheckAuthenticated()) return;

            if (!string.IsNullOrEmpty(_token) || !string.IsNullOrEmpty(_serviceTicket))
            {
                _provider.CheckToken( _context, _token, _serviceTicket);
            }
            else
            {
                _provider.Redirect2LoginPage(_context);
            }
        }

        private bool CheckAuthenticated()
        {
            if (_context.User == null || !_context.User.Identity.IsAuthenticated)
            {
                var appCookiePath = AuthConst.AppCookiePath(_appCode,_provider.CurrentProvider());
                if (_context.Request.Cookies[appCookiePath] == null)
                {
                    return false;
                }
                var encrypted = _context.Request.Cookies[appCookiePath].Value;
                var ticket = FormsAuthentication.Decrypt(encrypted);

                if (ticket == null)
                {
                    return false;
                }
                if (ticket.Expired)
                {
                    var httpCookie = _context.Response.Cookies[appCookiePath];
                    if (httpCookie != null)
                        httpCookie.Expires = DateTime.Now.AddDays(-1);
                    return false;
                }
                var userdataincookie = UserDataInCookie.ReadToObject(ticket.UserData);
                if (userdataincookie != null)
                {
                    AuthService.SetPrincipalBasedOnUserData(_context, userdataincookie);
                    return true;
                }
                else
                {
                    Log.Error("Invalid user data in FormsAuthentication cookie,ticket:" + JsonHelper.Serialize(ticket));
                    return false;
                }
            }
            else
            {
                return true;
            }

        }
    }
}
