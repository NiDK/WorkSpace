using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using System.Xml;
using PwC.C4.Configuration.Messager.Model;
using PwC.C4.Configuration.Messager.Service;
using PwC.C4.Configuration.Messager.Service.ServiceImp;
using PwC.C4.Infrastructure.Logger;
using PwC.C4.Membership;
using PwC.C4.Membership.WebExtension;

namespace PwC.C4.Configuration.Messager.Controllers
{
    [C4Security]
    public class HomeController : C4Controller
    {
        readonly LogWrapper _log = new LogWrapper();

       
        public ActionResult Index()
        {
            var model = ConfigurationService.Instance().GetAllConfig(0);
            return View(model);
        }

        public ActionResult ShowAllVersion(Guid configId, string appCode, int major, int index)
        {
            var totalCount = 0;
            var data = ConfigurationService.Instance()
                .ConfigurationDetail_ListByAppCode(configId, appCode, major, index, out totalCount);
            return Json(new {Data = data, TotalCount = totalCount}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowDetail(Guid id, string method)
        {
            var data = ConfigurationService.Instance().ConfigurationDetail_GetEntityById(id, method);
            return View(data);
        }

        public ActionResult AddVersion(Guid configId, string appCode, short major)
        {
            var data = ConfigurationService.Instance()
                .ConfigurationDetail_GetEntityByLastMinor(configId, appCode, major, "edit");
            return View(data);
        }

        [ValidateInput(false)]
        public ActionResult SubmitVersion(ConfigurationDetail detail)
        {
            var xml = new XmlDocument();
            xml.LoadXml(detail.Xml);
            detail.Content = xml;
            var result = ConfigurationService.Instance().ConfigurationDetail_AddNewMinor(detail);
            return Json(result);
        }

        [ValidateInput(false)]
        public ActionResult CheckXml(string xml)
        {
            var result = CommonService.CheckXml(xml);
            return Json(result);
        }

        public ActionResult Recovery(Guid configId, string appCode, short major, Guid id)
        {
            var result = ConfigurationService.Instance().RecoveryConfig(configId, appCode, major, id);
            return Json(result);
        }


        public ActionResult Admin()
        {
            return View();
        }


        public ActionResult InitConfig()
        {
            try
            {
                var lockFile = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "InitConfig.lock");
                if (!System.IO.File.Exists(lockFile))
                {
                    ConfigurationService.Instance().ConfigurationTruncateTable();
                    var data = Service.ConfigManager.GetAllConfig();
                    if (data == null)
                    {
                        return Json(new { Result = false, Message = "Load configurations error" });
                    }
                    var l = System.IO.File.Create(lockFile);
                    l.Dispose();
                    l.Close();
                    return Json(new { Result = true, Message = "Init configuration success!" });
                }
                else
                {
                    return Json(new { Result = false, Message = "Init configuration can not excuted ! Please delete file：<b>InitConfig.lock</b>!" });
                }
            }
            catch (Exception ex)
            {
                _log.Error("InitConfig error", ex);
                return Json(new { Result = false, Message = "Load configs error" });
            }
         
        }

        public ActionResult CreateConfigurationType(string configName,string configDesc)
        {
           var result = ConfigurationService.Instance()
                .ConfigurationType_Create(new ConfigurationType()
                {
                    Id = Guid.NewGuid(),
                    Name = configName,
                    Desc = configDesc,
                    Status = 0,
                    Creator = CurrentUser.StaffId
                });
            return Json(result);
        }

        public ActionResult CreateApplication(Guid configTypeId, string appCode)
        {
            var result = ConfigurationService.Instance().ConfigurationDetail_CreateApplication(configTypeId, appCode);
            return Json(result);
        }

        [ChildActionOnly]
        public ActionResult Menu()
        {
            return PartialView();
        }
    }
}