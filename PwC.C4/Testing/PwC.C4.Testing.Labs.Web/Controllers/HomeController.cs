using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using PwC.C4.DataService.Model;
using PwC.C4.Membership.WebExtension;
using PwC.C4.Testing.Labs.Web.Service;

namespace PwC.C4.Testing.Labs.Web.Controllers
{
    public class HomeController : BaseController
    {
        [C4Authorize("Admin","User","SuperAdmin")]
        public ActionResult Index()
        {
            
            return View();
        }
        [C4Authorize("Admin")]
        public ActionResult PostList()
        {
            return View();
        }
        [C4Authorize("User")]
        public ActionResult Detail()
        {
            return View();
        }
        [C4Security]
        public ActionResult Comments()
        {
            return View();
        }

        public ActionResult Categories()
        {
            return View();
        }

        public ActionResult Tags() {
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
        [C4Authorize("Admin", "SuperAdmin")]
        public ActionResult TestAdminView()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult TestUserView()
        {
            return View();
        }
    }
}