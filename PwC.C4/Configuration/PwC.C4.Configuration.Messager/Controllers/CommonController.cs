using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PwC.C4.Configuration.Messager.Controllers
{
    public class CommonController : Controller
    {
        // GET: Common
        [AllowAnonymous]
        public ActionResult NoAuthorize()
        {
            return View();
        }
    }
}