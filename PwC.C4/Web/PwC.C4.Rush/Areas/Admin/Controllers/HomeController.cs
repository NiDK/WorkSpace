using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PwC.C4.Infrastructure.Cache;
using PwC.C4.Membership;
using PwC.C4.Membership.WebExtension;
using PwC.C4.Rush.Models;
using PwC.C4.Rush.WcfService.Models;

namespace PwC.C4.Rush.Areas.Admin.Controllers
{
    [C4Security]
    public class HomeController : C4Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FormDesign(Guid formId)
        {
            ViewBag.FormId = formId;
            return View();
        }

        public ActionResult GetFormInfoForDesign(Guid formId)
        {
            var client = new RushServiceClient();
            var form = client.GetFormBaseInfo(formId);
            return Json(form, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFormList(string keyword,int page,int rows,string sort ,string order)
        {
            var client = new RushServiceClient();
            var total = 0;
            var imp = client.GetFormList(keyword, page, rows, sort, order, out total);
            var gridModel = new GridModel<FormMain>()
            {
                total = total,
                rows = imp
            };
            return Json(gridModel);
        }

        public ActionResult GetLayoutList()
        {
            var client = new RushServiceClient();
            var layouts = client.GetFormLayoutList();
            return Json(layouts,JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveFormBaseInfo(FormMain form, bool? delete)
        {
            var client = new RushServiceClient();
            form.CreateBy = CurrentUser.StaffName;
            form.ModifyBy = form.CreateBy;
            if (form.Id != Guid.Empty && (delete ?? false))
            {
                var deleteResult = client.DeleteFormBaseInfo(form.Id, form.ModifyBy);
                return Json(new {Result = deleteResult > 0});
            }

            var isNew = form.Id == Guid.Empty;

            var formId = client.SaveFormBaseInfo(form);
            return Json(new {isNew = isNew, formId = formId});
        }

        public ActionResult GetFormBaseInfo(Guid formId)
        {
            var client = new RushServiceClient();
            var form = client.GetFormBaseInfo(formId);
            return Json(form,JsonRequestBehavior.AllowGet);
        }
        [ValidateInput(false)]
        public ActionResult SaveFormStructure(Guid formId,List<string> props,string javascript,string styles,List<FormControl>  formControls)
        {
            var client = new RushServiceClient();
            var p = props != null ? string.Join(",", props) : "";
            var data = client
                .UpdateStructure(formId, CurrentUser.StaffName, p, javascript, styles, formControls);
            return Json(data > 0);
        }

       
        public ActionResult PreviewForm(string aliasName, string previewId, string dataId)
        {
            var ar = CacheHelper.GetCacheItem<FromRender>("PreId-" + previewId);
            var client = new RushServiceClient();
            var form = client.RenderForm(dataId, aliasName, ar.FormName);
            ViewBag.DataId = dataId;
            form.Styles = ar.Styles;
            form.Javascripts = ar.Javascripts;
            form.Controls = ar.Controls;
            Response.Headers.Add("X-XSS-Protection", "0");
            return View("~/Views/Shared/Index.cshtml",form);
        }
        [ValidateInput(false)]
        public ActionResult BuildPreview(string javascript, string styles,
            List<FormControl> formControls,string props )
        {
            var dId = Guid.NewGuid().ToString("N");
            var formRender = new FromRender
            {
                Styles = styles,
                Javascripts = javascript,
                Controls = formControls,
                FormName = props
            };
            if (CacheHelper.SetCacheItem("PreId-" + dId, formRender))
                return Json(dId);
            return Json("");
        }

        public ActionResult EnableForm()
        {
            return Json("");
        }
    }
}