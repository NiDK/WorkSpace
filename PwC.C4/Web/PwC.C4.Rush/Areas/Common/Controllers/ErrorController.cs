using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PwC.C4.Membership.WebExtension;

namespace PwC.C4.Rush.Areas.Common.Controllers
{
    [AllowAnonymous]
    public class ErrorController : C4Controller
    {
        // GET: Common/Error
        public ActionResult NoAuthorize()
        {
            return View();
        }

        public ActionResult SystemError(string msg)
        {
            ViewBag.Message = msg;
            return View();
        }
    }
}