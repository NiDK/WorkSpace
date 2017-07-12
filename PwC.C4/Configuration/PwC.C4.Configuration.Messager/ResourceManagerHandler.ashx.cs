using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace PwC.C4.Configuration.Messager {
    public class ResourceManagerHandler : IHttpHandler {
        private const string NoAppPath = "General";

        private string GetDownloadUrl(string sectionName, string applicationName, int major, int minor) {
            string folder = ConfigurationManager.AppSettings["httpResourceFolder"];
            return folder + "/" + sectionName + "/" + applicationName + "/" + major + "/" + sectionName + "." + minor;
        }

        private void saveConfig(string sectionName, string applicationName, int major, int minor, byte[] buffer, string OperatorID) {
            MemoryStream stream = new MemoryStream(buffer);
            XmlDocument doc = new XmlDocument();
            doc.Load(stream);

            string[] names = sectionName.Split(".".ToCharArray());
            doc.DocumentElement.SetAttribute("resourceSetName", names[0]);
            doc.DocumentElement.SetAttribute("culture", names[1]);
            doc.DocumentElement.SetAttribute("majorVersion", major.ToString());
            doc.DocumentElement.SetAttribute("minorVersion", minor.ToString());

            string folder = ConfigurationManager.AppSettings["resourceFolder"];
            string fileName = folder + "\\" + sectionName + "\\" + applicationName + "\\" + major + "\\" + sectionName + "." + minor;
            doc.Save(fileName);

            addLog(sectionName, applicationName, major, minor, OperatorID);
        }

        private void addLog(string sectionName, string applicationName, int major, int minor, string OperatorID) {
            string folder = ConfigurationManager.AppSettings["resourceFolder"];
            string fileName = folder + "\\" + sectionName + "\\" + applicationName + "\\" + major + "\\log.txt";
            FileStream fs;
            if (File.Exists(fileName)) {
                fs = new FileStream(fileName, FileMode.Append, FileAccess.Write);
            }
            else {
                fs = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write);
            }
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(string.Format("minor={0};operatorid={1};datetime={2}", minor, OperatorID, DateTime.Now));
            sw.Close();
            fs.Close();
        }

        private RemoteConfigManagerDTO getAllConfigs() {
            RemoteConfigManagerDTO result = new RemoteConfigManagerDTO();
            try {
                result.Operation.Result = true;

                string folder = ConfigurationManager.AppSettings["resourceFolder"];
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
                string[] configs = Directory.GetDirectories(folder);
                if (configs.Length == 0)
                    return result;
                foreach (string config in configs) {
                    string sectionName = config.Substring(folder.Length + 1);
                    result.RemoteConfigSections.AddSection(sectionName, 0, 0);
                }
            }
            catch (Exception ex) {
                result.Operation.Result = false;
                result.Operation.ResultInfo = ex.ToString();
            }
            return result;
        }

        private RemoteConfigManagerDTO getApplicatinos(RemoteConfigManagerDTO remoteConfigManager) {
            RemoteConfigManagerDTO result = new RemoteConfigManagerDTO();
            try {
                result.Operation.Result = true;

                string folder = ConfigurationManager.AppSettings["resourceFolder"];
                folder = Path.Combine(folder, remoteConfigManager.Operation.Condition);
                if (!Directory.Exists(folder)) {
                    return result;
                }
                string[] applications = Directory.GetDirectories(folder);
                if (applications.Length == 0)
                    return result;
                foreach (string application in applications) {
                    string sectionName = application.Substring(folder.Length + 1);
                    result.RemoteConfigSections.AddSection(sectionName, 0, 0);
                }
            }
            catch (Exception ex) {
                result.Operation.Result = false;
                result.Operation.ResultInfo = ex.ToString();
            }
            return result;
        }

        private RemoteConfigManagerDTO getMajors(RemoteConfigManagerDTO remoteConfigManager) {
            RemoteConfigManagerDTO result = new RemoteConfigManagerDTO();
            try {
                result.Operation.Result = true;

                string folder = ConfigurationManager.AppSettings["resourceFolder"];
                folder = Path.Combine(folder, remoteConfigManager.Operation.Condition);
                if (!Directory.Exists(folder)) {
                    return result;
                }
                string[] majorVersions = Directory.GetDirectories(folder);
                if (majorVersions.Length == 0)
                    return result;
                foreach (string majorVersion in majorVersions) {
                    int major;
                    if (int.TryParse(majorVersion.Substring(folder.Length + 1), out major)) {
                        result.RemoteConfigSections.AddSection(remoteConfigManager.Operation.Condition, major, 0);
                    }
                }
            }
            catch (Exception ex) {
                result.Operation.Result = false;
                result.Operation.ResultInfo = ex.ToString();
            }
            return result;
        }

        private RemoteConfigManagerDTO getMinors(RemoteConfigManagerDTO remoteConfigManager) {
            RemoteConfigManagerDTO result = new RemoteConfigManagerDTO();
            try {
                result.Operation.Result = true;

                string folder = ConfigurationManager.AppSettings["resourceFolder"];
                folder = Path.Combine(folder, remoteConfigManager.Operation.Condition);
                if (Directory.Exists(folder)) {
                    string[] minorVersions = Directory.GetFiles(folder);
                    if (minorVersions.Length == 0)
                        return result;
                    foreach (string minorVersion in minorVersions) {
                        string[] strs = remoteConfigManager.Operation.Condition.Split("\\".ToCharArray());
                        string sectionName = strs[0];
                        string applicationName = strs[1];
                        int major = Convert.ToInt32(strs[2]);

                        string fileName = minorVersion.Substring(folder.Length + 1);
                        strs = fileName.Split(".".ToCharArray());
                        int minor;
                        if (int.TryParse(strs[strs.Length - 1], out minor)) {
                            string downloadUrl = GetDownloadUrl(sectionName, applicationName, major, minor);
                            result.RemoteConfigSections.AddSection(Path.Combine(sectionName, applicationName), major, minor, downloadUrl);
                        }
                    }

                    string logFileName = folder + @"\log.txt";
                    if (File.Exists(logFileName)) {
                        FileStream fs = new FileStream(logFileName, FileMode.Open, FileAccess.Read);
                        StreamReader sr = new StreamReader(fs);
                        result.Operation.Log = Encoding.Default.GetBytes(sr.ReadToEnd());
                        sr.Close();
                        fs.Close();
                    }
                }
            }
            catch (Exception ex) {
                result.Operation.Result = false;
                result.Operation.ResultInfo = ex.ToString();
            }
            return result;
        }

        private RemoteConfigManagerDTO createMinor(RemoteConfigManagerDTO remoteConfigManager) {
            RemoteConfigManagerDTO result = new RemoteConfigManagerDTO();
            try {
                result.Operation.Result = true;

                string folder = ConfigurationManager.AppSettings["resourceFolder"];
                folder = Path.Combine(folder, remoteConfigManager.Operation.Condition);
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string[] strs = remoteConfigManager.Operation.Condition.Split("\\".ToCharArray());
                string sectionName = strs[0];
                string applicationName = strs[1];
                int major = Convert.ToInt32(strs[2]);

                string[] minorVersions = Directory.GetFiles(folder);
                if (minorVersions.Length == 0) {
                    saveConfig(sectionName, applicationName, major, 1, remoteConfigManager.Operation.Value, remoteConfigManager.Operation.OperatorID);
                    result.Operation.ResultInfo = GetDownloadUrl(sectionName, applicationName, major, 1);
                    return result;
                }

                int maxMinor = 1;
                foreach (string minorVersion in minorVersions) {
                    string fileName = minorVersion.Substring(folder.Length + 1);
                    strs = fileName.Split(".".ToCharArray());
                    int minor;
                    if (int.TryParse(strs[strs.Length - 1], out minor)) {
                        maxMinor = Math.Max(maxMinor, minor);
                    }
                }

                saveConfig(sectionName, applicationName, major, maxMinor + 1, remoteConfigManager.Operation.Value, remoteConfigManager.Operation.OperatorID);
                result.Operation.ResultInfo = GetDownloadUrl(sectionName, applicationName, major, maxMinor + 1);
                return result;
            }
            catch (Exception ex) {
                result.Operation.Result = false;
                result.Operation.ResultInfo = ex.ToString();
            }
            return result;
        }

        private RemoteConfigManagerDTO deleteConfig(RemoteConfigManagerDTO remoteConfigManager) {
            RemoteConfigManagerDTO result = new RemoteConfigManagerDTO();
            try {
                result.Operation.Result = true;

                string folder = ConfigurationManager.AppSettings["resourceFolder"];
                folder = Path.Combine(folder, remoteConfigManager.Operation.Condition);
                if (Directory.Exists(folder)) {
                    string bakFolder = ConfigurationManager.AppSettings["bakFolder"];
                    bakFolder = Path.Combine(bakFolder, remoteConfigManager.Operation.Condition);
                    if (!Directory.Exists(bakFolder)) {
                        Directory.CreateDirectory(bakFolder);
                    }
                    bakFolder = Path.Combine(bakFolder, DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss"));
                    Directory.Move(folder, bakFolder);
                }
            }
            catch (Exception ex) {
                result.Operation.Result = false;
                result.Operation.ResultInfo = ex.ToString();
            }
            return result;
        }

        private RemoteConfigManagerDTO getAllLastVersion() {
            RemoteConfigManagerDTO result = new RemoteConfigManagerDTO();
            try {
                result.Operation.Result = true;

                string folder = ConfigurationManager.AppSettings["resourceFolder"];
                if (Directory.Exists(folder)) {
                    string[] sections = Directory.GetDirectories(folder);
                    foreach (string section in sections) {
                        string sectionName = section.Substring(folder.Length + 1);
                        string[] applications = Directory.GetDirectories(section);
                        foreach (string application in applications) {
                            string applicationName = application.Substring(section.Length + 1);
                            string[] majorVersions = Directory.GetDirectories(application);
                            foreach (string majorVersion in majorVersions) {
                                int major;
                                if (int.TryParse(majorVersion.Substring(application.Length + 1), out major)) {
                                    int maxMinor = 0;
                                    string[] minorVersions = Directory.GetFiles(majorVersion);
                                    foreach (string minorVersion in minorVersions) {
                                        string fileName = minorVersion.Substring(majorVersion.Length + 1);
                                        string[] strs = fileName.Split(".".ToCharArray());
                                        int minor;
                                        if (int.TryParse(strs[strs.Length - 1], out minor)) {
                                            maxMinor = Math.Max(maxMinor, minor);
                                        }
                                    }
                                    string url = GetDownloadUrl(sectionName, applicationName, major, maxMinor);
                                    result.RemoteConfigSections.AddSection(Path.Combine(sectionName, applicationName), major, maxMinor, url);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                result.Operation.Result = false;
                result.Operation.ResultInfo = ex.ToString();
            }
            return result;
        }

        private RemoteConfigManagerDTO manager(RemoteConfigManagerDTO remoteConfigManager) {
            RemoteConfigManagerDTO result;

            switch (remoteConfigManager.Operation.Command) {
                case "getAllConfigs": {
                        return getAllConfigs();
                    }
                case "getApplications": {
                        return getApplicatinos(remoteConfigManager);
                    }
                case "getMajors": {
                        return getMajors(remoteConfigManager);
                    }
                case "getMinors": {
                        return getMinors(remoteConfigManager);
                    }
                case "createMinor": {
                        return createMinor(remoteConfigManager);
                    }
                case "deleteConfig": {
                        return deleteConfig(remoteConfigManager);
                    }
                case "getAllLastVersion": {
                        return getAllLastVersion();
                    }
                default: {
                        result = new RemoteConfigManagerDTO();
                        result.Operation.Result = false;
                        return result;
                    }
            }
        }

        public void ProcessRequest(HttpContext context) {
            if (context.Request.InputStream.Length == 0) {
                return;
            }
            XmlSerializer xser = new XmlSerializer(typeof(RemoteConfigManagerDTO));
            RemoteConfigManagerDTO remoteConfigManager = (RemoteConfigManagerDTO)xser.Deserialize(context.Request.InputStream);

            RemoteConfigManagerDTO result = manager(remoteConfigManager);

            context.Response.ContentType = "text/xml";
            xser.Serialize(context.Response.OutputStream, result);
        }

        public bool IsReusable {
            get {
                return false;
            }
        }
    }

}
