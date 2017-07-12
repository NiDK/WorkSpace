using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PwC.C4.Dfs.Web.Auth;
using PwC.C4.Infrastructure.Helper;

namespace PwC.C4.Dfs.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string keyspace,string appcode,string fileId)
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Validate()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Validate(string code, string returnUrl)
        {
            ModelState.Clear();

            HttpCookie cookie = Request.Cookies.Get(AuthorizationHelper.EncryptedCaptchaCookieName);
            if (cookie == null || cookie.Value == null)
            {
                ModelState.AddModelError("code", "验证码过期,请返回重新输入");
            }
            else
            {
                string codeInCookie = EncryptHelper.Decode(cookie.Value);
                if (!string.Equals(code, codeInCookie, StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError("code", "验证码输入错误");
                }
                else
                {
                    Response.Cookies.Add(new HttpCookie(AuthorizationHelper.CaptchaCookieName, code));
                    Response.Cookies.Add(cookie);
                    Response.Redirect(returnUrl);
                }
            }

            return View();
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult GetValidateCode()
        {
            string image = AppDomain.CurrentDomain.BaseDirectory + @"Content\Images\validate.gif";
            return new ImageCheck(image, AuthorizationHelper.EncryptedCaptchaCookieName);
        }

        public ActionResult List()
        {
            return View();
        }
    }
}