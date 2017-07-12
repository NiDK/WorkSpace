using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PwC.C4.Common.Provider;

namespace PwC.C4.Sketch.Controllers
{
    public class CommonController : Controller
    {
        // GET: Common
        [AllowAnonymous]
        public ActionResult NoAuthorize()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult SystemError()
        {
            return View();
        }
        [ChildActionOnly]
        public ActionResult Menu()
        {
            var menus = CurrentUserProvider.MenuRoles();
            return PartialView(menus);
        }
    }
}