using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using ApplicationCenter.ClientClass;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Logger;


namespace PwC.C4.Common.Provider
{
    public class C4HttpModule : IHttpModule
    {
        static readonly LogWrapper log = new LogWrapper();
        public void Init(HttpApplication context)
        {
            context.EndRequest += OnApplicationEndRequest;
            context.AuthorizeRequest += OnAuthorizeRequest;
            context.AuthenticateRequest += OnAuthenticateRequest;
        }

        private static void OnAuthenticateRequest(object sender, EventArgs e)
        {
            var url = HttpContext.Current.Request.RawUrl;
            try
            {
                try
                {
                    if (AppSettings.Instance.IsInUrlWhiteList(url))
                    {
                        return;
                    }
                }
                catch (Exception ee)
                {
                    log.Error("IsInUrlWhiteList error,url:" + url, ee);
                }
                if (url.Contains("/PwC.Configuration/Download/"))
                {
                    return;
                }
                if (Regex.IsMatch(url, @"(/[^/#?]+)*\.(?:svc)"))
                {
                    return;
                }
                if (Regex.IsMatch(url, @"(/[^/#?]+)*\.(?:ashx)"))
                {
                    return;
                }
                var hra = new HttpRequestAuthenticate(HttpContext.Current);
                hra.Authenticate();
            }
            catch (System.Threading.ThreadAbortException)
            {
                //log.Error("Current havn't login Error,login url:" + url, ee);
            }
            catch (Exception ee)
            {
                log.Error("OnAuthenticateRequest Error,please check machine key's setting in web.config or reset login device.login url:" + HttpContext.Current.Request.Url.AbsoluteUri, ee);
                HttpContext.Current.ApplicationInstance.Response.Redirect(
                    AppSettings.Instance.GetAuthenticateErrorRequestUrl());
            }
            

        }


        private static void OnAuthorizeRequest(object sender, EventArgs e)
        {
            var url = HttpContext.Current.Request.RawUrl;
            try
            {
                if (AppSettings.Instance.IsInUrlWhiteList(url))
                {
                    return;
                }
                if (Regex.IsMatch(url, @"(/[^/#?]+)*\.(?:svc)"))
                {
                    return;
                }
                if (Regex.IsMatch(url, @"(/[^/#?]+)*\.(?:ashx)"))
                {
                    return;
                }
                if (HttpContext.Current.User == null || !HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    log.Error("(HttpContext.Current.User == null || !HttpContext.Current.User.Identity.IsAuthenticated,Login Url:"
                        + HttpContext.Current.Request.Url.AbsoluteUri);
                    UnAuthorized();
                }
            }
            catch (Exception ee)
            {
                log.Error("Ri OnAuthorizeRequest Error,please check machine key's setting in web.config,Login Url:" + HttpContext.Current.Request.Url.AbsoluteUri, ee);
                HttpContext.Current.ApplicationInstance.Response.Redirect(
                    AppSettings.Instance.GetAuthenticateErrorRequestUrl());
            }
        }



        public static void UnAuthorized()
        {
            var pageUrl = ConfigurationManager.AppSettings["UnAuthorizedPageUrl"] as string;
            if (string.IsNullOrEmpty(pageUrl))
                if (pageUrl != null) HttpContext.Current.ApplicationInstance.Response.Redirect(pageUrl);
        }


        // If the request was unauthorized, add the WWW-Authenticate header 
        // to the response.
        private static void OnApplicationEndRequest(object sender, EventArgs e)
        {


        }

        public void Dispose()
        {
        }
    }
}
