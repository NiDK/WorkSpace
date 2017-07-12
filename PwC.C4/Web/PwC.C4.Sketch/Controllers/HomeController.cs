using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PwC.C4.Metadata.Service;
using PwC.C4.Sketch.Models.Enum;
using PwC.C4.Sketch.Service.Web;

namespace PwC.C4.Sketch.Controllers
{
    [C4Authorize(Roles.Admin, Roles.AppAdmin, Roles.Editor, Roles.User)]
    public class HomeController : C4Controller
    {

        public ActionResult Index()
        {
            ViewBag.Menu = "Home";
            return View();
        }

        public ActionResult Search()
        {
            ViewBag.Menu = "Search";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Documents()
        {
            ViewBag.Message = "Documents";

            return View();
        }

        public ActionResult Category()
        {
            var cates = HtmlCategoryService.Instance().HtmlCategory_GetByGroup("Default");
            return View(cates);
        }
    }
}