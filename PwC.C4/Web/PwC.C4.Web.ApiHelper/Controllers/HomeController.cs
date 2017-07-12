using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace PwC.C4.Web.ApiHelper.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }

        public ActionResult DataPicker()
        {
            return View();
        }

        public ActionResult PeoplePicker()
        {
            return View();
        }
    }
}
