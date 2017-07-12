using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using log4net;
using PwC.C4.Configuration.Messager.Model;
using PwC.C4.Configuration.Messager.Service.ServiceImp;
using PwC.C4.Infrastructure.Logger;
using PwC.C4.Membership;
using WebGrease.Css.Extensions;

namespace PwC.C4.Configuration.Messager.Service
{
    public static class ConfigManager
    {
        static readonly LogWrapper Log = new LogWrapper();
        public static PageModel<ConfigInfo> GetAllConfig()
        {
            try
            {
                var pageModel = new PageModel<ConfigInfo> { Datas = new List<ConfigInfo>() };
                var folder = ConfigurationManager.AppSettings["publishFolder"];
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
                var configs = Directory.GetDirectories(folder);
                if (configs.Length == 0)
                    return new PageModel<ConfigInfo>();
                foreach (var config in configs)
                {
                    var con = new ConfigInfo { ConfigName = new DirectoryInfo(config).Name };

                    var coninfo = new ConfigurationType();
                    coninfo.Id = Guid.NewGuid();
                    coninfo.Name = con.ConfigName;
                    coninfo.Creator = CurrentUser.StaffId + " By Init";
                    coninfo.Status = 0;
                    coninfo.Desc = "Empty";
                    ConfigurationService.Instance().ConfigurationType_Create(coninfo);

                    var apps = Directory.GetDirectories(Path.Combine(folder, config));
                    if (apps.Length > 0)
                    {
                        var sett = new List<ConfigSetting>();
                        apps.ForEach(app =>
                        {
                            var cons = new ConfigSetting { AppCode = new DirectoryInfo(app).Name };

                            var majors = Directory.GetDirectories(Path.Combine(folder, config, cons.AppCode));
                            if (majors.Length > 0)
                            {
                                var ms = new List<short>();
                                majors.ForEach(c =>
                                {
                                    var name = new DirectoryInfo(c).Name;
                                    short s = 0;
                                    if (short.TryParse(name, out s))
                                    {
                                        ms.Add(s);
                                    }
                                });
                                cons.Major = ms.Max();
                            }

                            var configFiles =
                                Directory.GetFiles(Path.Combine(folder, config, cons.AppCode, cons.Major.ToString()));
                            var ss = new List<int>();
                            configFiles.ForEach(c =>
                            {
                                var extension = Path.GetExtension(c);
                                if (extension != null)
                                {
                                    var iss = extension.Replace(".", "");
                                    var s = 0;
                                    if (int.TryParse(iss, out s))
                                    {
                                        ss.Add(s);
                                        var i = new ConfigurationDetail();
                                        i.AppCode = cons.AppCode;
                                        i.Content = GetFileContent(con.ConfigName, cons.AppCode, cons.Major.ToString(),
                                            s.ToString());
                                        i.Creator = CurrentUser.StaffId+" By Init";
                                        i.Major = cons.Major;
                                        i.ConfigId = coninfo.Id;
                                        i.Minor = s;
                                        i.ConfigName = coninfo.Name;
                                        i.Status = 0;
                                        if (i.Content != null)
                                        {
                                            ConfigurationService.Instance().ConfigurationDetail_Create(i);
                                        }
                                    }
                                }
                            });
                            cons.Minor = ss.Max();
                            sett.Add(cons);
                        });

                        con.ConfigSettings = sett;
                    }
                    pageModel.Datas.Add(con);
                    pageModel.TotalCount++;
                }
                return pageModel;
            }
            catch (Exception ex)
            {
                Log.Error("GetAllConfig error", ex);
                return null;
            }
            
        }
        private static XmlDocument GetFileContent(string conName, string appcode, string major, string minor)
        {
            try
            {
                var p = ConfigurationManager.AppSettings["publishFolder"];
                var filename = conName + "." + minor;
                var filePath = Path.Combine(p, conName, appcode, major, filename);
                var xml = new XmlDocument();
                xml.Load(filePath);
                return xml;
            }
            catch (Exception ee)
            {
                Log.Error(
                    "GetFileContent error,conName:" + conName + ",appcode:" + appcode + ",major:" + major + ",minor:" +
                    minor, ee);
                return null;
            }

        }
    }

    
}
