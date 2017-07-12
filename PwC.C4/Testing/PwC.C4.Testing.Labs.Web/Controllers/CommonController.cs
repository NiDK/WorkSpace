using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PwC.C4.Testing.Labs.Web.Controllers
{
    public class CommonController : Controller
    {
        // GET: Common
        public ActionResult NoAuthorize()
        {
            return View();
        }
    }
}