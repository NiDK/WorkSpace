using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Highlight;
using Highlight.Engines;
using log4net;
using PwC.C4.Configuration.Messager.Model;
using PwC.C4.Configuration.Messager.Service.Interface;
using PwC.C4.Configuration.Messager.Service.Persistance;
using PwC.C4.Infrastructure.Logger;
using PwC.C4.Membership;

namespace PwC.C4.Configuration.Messager.Service.ServiceImp
{
    public class ConfigurationService : IConfigurationService
    {

        #region Singleton
        readonly LogWrapper _log = new LogWrapper();
        private static ConfigurationService _instance = null;
        private static readonly object LockHelper = new object();

        public ConfigurationService()
        {
        }

        public static IConfigurationService Instance()
        {
            if (_instance == null)
            {
                lock (LockHelper)
                {
                    if (_instance == null)
                    {
                         _instance = new ConfigurationService();
                    }
                       
                }
            }
            return _instance;
        }

#if DEBUG

        public static ConfigurationService DebugInstance()
        {
            if (_instance == null)
            {
                lock (LockHelper)
                {
                    if (_instance == null)
                    {
                        _instance = new ConfigurationService();
                    }
                        
                }
            }
            return _instance;
        }

#endif

        #endregion

        public int ConfigurationDetail_Create(ConfigurationDetail detail)
        {
            detail.Content = WriteXmlFix(detail.Content, detail.ConfigName, detail.Major, detail.Minor ?? 1);
            return ConfigurationDao.ConfigurationDetail_Create(detail);
        }

        public int ConfigurationType_Create(ConfigurationType type)
        {
            return ConfigurationDao.ConfigurationType_Create(type);
        }

        public int ConfigurationDetail_GetMinor(Guid configId, string appCode, short major)
        {
            return ConfigurationDao.ConfigurationDetail_GetMinor(configId, appCode, major);
        }

        public ConfigurationDetail ConfigurationDetail_GetEntity(Guid configId, string appCode, short major, int minor, string method)
        {
            var data = ConfigurationDao.ConfigurationDetail_Entity(configId, appCode, major, minor);
            data.Xml = ReadXmlFix(data.Content, data.ConfigName, method);
            return data;
        }

        public ConfigurationDetail ConfigurationDetail_GetEntityById(Guid id, string method)
        {
            var data = ConfigurationDao.ConfigurationDetail_Entity(id);
            data.Xml = ReadXmlFix(data.Content, data.ConfigName, method);
            return data;
        }

        public ConfigurationDetail ConfigurationDetail_GetEntityByLastMinor(Guid configId, string appCode, short major, string method)
        {
            var lastMinor = ConfigurationDetail_GetMinor(configId, appCode, major);
            var data = ConfigurationDetail_GetEntity(configId, appCode, major, lastMinor, method);
            return data;
        }


        public int ConfigurationDetail_AddNewMinor(ConfigurationDetail detail)
        {
            var minor = ConfigurationDetail_GetMinor(detail.ConfigId, detail.AppCode, detail.Major);
            if (minor > 0)
                minor++;
            detail.Minor = minor;
            return ConfigurationDetail_Create(detail);
        }

        public int ConfigurationDetail_AddNewMajor(ConfigurationDetail detail)
        {
            return ConfigurationDetail_Create(detail);
        }

        public List<ConfigurationType> ConfigurationType_ListByPaging(int index,  out int totalCount)
        {
            return ConfigurationDao.ConfigurationType_ListByPaging(index, GetPageSize(), out totalCount);
        }

        public List<ConfigurationDetail> ConfigurationDetail_ListByPaging(Guid configId, int index, out int totalCount)
        {
            return ConfigurationDao.ConfigurationDetail_ListByPaging(configId, index, GetPageSize(), out totalCount);
        }

        public List<ConfigurationDetail> ConfigurationDetail_ListByAppCode(Guid configId, string appCode,int major, int index, out int totalCount)
        {
            return ConfigurationDao.ConfigurationDetail_ListByAppCode(configId, appCode, major, index, GetPageSize(), out totalCount);
        }

        public PageModel<ConfigurationType> GetAllConfig(int index)
        {
            var model = new PageModel<ConfigurationType>();
            int totalCount = 0;
            var typs = ConfigurationType_ListByPaging(index, out totalCount);
            model.TotalCount = totalCount;
            model.Datas = typs;

            model.Datas.ForEach(c =>
            {
                int dcount = 0;
                c.ConfigurationDetails = new PageModel<ConfigurationDetail>
                {
                    Datas = ConfigurationDao.ConfigurationDetail_ListByPaging(c.Id, index, GetPageSize(), out dcount),
                    TotalCount = dcount
                };
            });

            return model;
        }

        public int RecoveryConfig(Guid configId, string appCode, short major, Guid id)
        {
            var detail = ConfigurationDetail_GetEntityById(id, "edit");
            return ConfigurationDetail_AddNewMinor(detail);
        }

        private static int GetPageSize()
        {
            var pageSize = ConfigurationManager.AppSettings["PageSize"];
            var page = -1;
            int.TryParse(pageSize, out page);
            return page;
        }


        public string ReadXmlFix(XmlDocument xml, string configName, string method)
        {
            if (method != "download")
            {
                var selectSingleNode = (XmlElement) xml.SelectSingleNode(configName);
                selectSingleNode?.RemoveAllAttributes();
            }

            if (method == "edit" || method == "download")
            {
                return xml.Beautify();
            }
            else
            {
                var highlighter = new Highlighter(new HtmlEngine());
                return highlighter.Highlight("XML", xml.Beautify());
            }

        }

        public XmlDocument WriteXmlFix(XmlDocument xml, string configName, short major, int minor)
        {
            var selectSingleNode = (XmlElement)xml.SelectSingleNode(configName);
            selectSingleNode?.RemoveAllAttributes();
            if (selectSingleNode != null)
            {
                selectSingleNode.SetAttribute("majorVersion", major.ToString());
                selectSingleNode.SetAttribute("minorVersion", minor.ToString());
            }
            return xml;
        }

        public bool SaveFileToServer(ConfigurationDetail detail)
        {
            try
            {
                var fileName = @"\" + detail.ConfigName + "." +detail.Minor;
                var path = detail.ConfigName + @"\" + detail.AppCode + @"\" + detail.Major;
                var p = ConfigurationManager.AppSettings["publishFolder"] + @"\" + path;
                if (!Directory.Exists(p))
                {
                    Directory.CreateDirectory(p);
                }
                detail.Content.Save(p+ fileName);
                return true;
            }
            catch (Exception ex)
            {
                _log.Error("SaveFileToServer error",ex);
                return false;
            }
            
        }

        public void ConfigurationTruncateTable()
        {
            ConfigurationDao.ConfigurationTruncateTable();
        }

        public int ConfigurationDetail_CreateApplication(Guid configTypeId, string appCode)
        {
            try
            {
                var detail = new ConfigurationDetail()
                {
                    AppCode = appCode,
                    ConfigId = configTypeId,
                    Creator = CurrentUser.StaffId,
                    Status = 0
                };
                return ConfigurationDao.ConfigurationDetail_CreateApplication(detail);
            }
            catch (Exception ee)
            {
                _log.Error("ConfigurationDetail_CreateApplication error",ee);
                return 0;
            }
            
        }

        public void InitConfigurationByNotification()
        {
            try
            {
                var machine = Dns.GetHostName();
                var key = "ConfigLast-" + machine;
                var cons = ConfigurationDao.Notification_ForConfigurationUpdate(machine, key);
                if (cons.Any())
                {
                    cons.ForEach(config =>
                    {
                        try
                        {
                            SaveFileToServer(config);
                        }
                        catch (Exception ee)
                        {
                            _log.Error("SaveFileToServer error", ee);
                        }
                    });
                }
            }
            catch (Exception ee)
            {
                _log.Error("InitConfigurationByNotification error",ee);
            }
           
        }
    }
}
