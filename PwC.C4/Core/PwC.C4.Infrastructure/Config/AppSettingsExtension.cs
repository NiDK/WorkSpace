using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using PwC.C4.Configuration.Data;
using PwC.C4.Infrastructure.BaseLogger;
using PwC.C4.Infrastructure.Exceptions;

namespace PwC.C4.Infrastructure.Config
{

    public static class AppSettingsExtension
    {
        private static List<string> _urlWhtieList;

        static BaseLogger.LogWrapper _log = new LogWrapper();

        private static string _authenticateMode;

        private static string _globalization;

        public static SettingNode GetNode(this AppSettings settings, string groupName, string key)
        {
            if (AppSettings.NodesCache.Count == 0)
            {
                AppSettings.LoadCache(AppSettings.Instance);
            }

            if (string.IsNullOrEmpty(groupName))
                groupName = "0";
            var dicKey = string.Format(AppSettings.DirectoryKeyFormat, groupName, key);
            SettingNode node;
            AppSettings.NodesCache.TryGetValue(dicKey, out node);
            return node;
        }
        public static SettingNode GetNode(this AppSettings settings, string key)
        {
            return settings.GetNode(null, key);
        }
        public static string GetStringOrDefault(this SettingNode node, string defaultValue)
        {
            if (node == null)
                return defaultValue;
            return node.Value ?? defaultValue;
        }
        public static int GetIntOrDefault(this SettingNode node, int value)
        {
            var s = node.GetStringOrDefault(string.Empty);
            return string.IsNullOrEmpty(s) ? value : int.Parse(s);
        }
        public static bool GetBoolOrDefault(this SettingNode node, bool value)
        {
            var s = node.GetStringOrDefault(string.Empty);
            return string.IsNullOrEmpty(s) ? value : bool.Parse(s);
        }
        public static string GetContentOrDefault(this SettingNode node, string value)
        {
            return node == null ? value : (node.Content ?? value);
        }

        public static SettingGroup GetGroup(this AppSettings settings, string groupName)
        {
            if (settings != null && settings.Groups != null)
            {
                return (from g in settings.Groups
                        where g.GroupName.Equals(groupName, StringComparison.CurrentCultureIgnoreCase)
                        select g).
                    FirstOrDefault();
            }
            else
            {
                return null;
            }

        }
        public static IEnumerable<SettingNode> GetNodes(this AppSettings settings, string groupName, string key)
        {
            foreach (var group in settings.Groups)
            {
                if (string.IsNullOrEmpty(groupName) ||
                    group.GroupName.Equals(groupName, StringComparison.CurrentCultureIgnoreCase))
                {
                    foreach (var node in group.Nodes)
                    {
                        if (node.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase))
                        {
                            yield return node;

                        }
                    }
                }

            }

        }

        [Obsolete("Please to use GetConntectStringV2(this AppSettings settings, string connName)")]
        public static string GetAppConntectString(this AppSettings settings)
        {
            var connName = settings.GetNode(ConfigConstValues.SystemNodeName, ConfigConstValues.AppConnStrName).Value;
            var appVirtualPath = HttpRuntime.AppDomainAppVirtualPath;
            if (string.IsNullOrEmpty(connName))
            {
                throw new NoConntectNameException("ConnName:" + ConfigConstValues.AppConnStrName);
            }
            if (!string.IsNullOrEmpty(appVirtualPath))
            {
                var webC = WebConfigurationManager.OpenWebConfiguration(appVirtualPath).ConnectionStrings;
                if (webC != null && webC.ConnectionStrings[connName]!=null)
                {
                    return webC.ConnectionStrings[connName].ConnectionString;
                }
                return settings.GetNode(ConfigConstValues.SystemNodeName, connName).Value;
            }
            else
            {
                return settings.GetNode(ConfigConstValues.SystemNodeName, connName).Value;
            }
        }

        [Obsolete("Please to use GetConntectStringV2(this AppSettings settings, string connName)")]
        public static string GetConntectString(this AppSettings settings, string connName)
        {
            var name = settings.GetNode(ConfigConstValues.SystemNodeName, connName).Value;
            if (string.IsNullOrEmpty(name))
            {
                throw new NoConntectNameException("ConnName:" + connName);
            }
            var appVirtualPath = HttpRuntime.AppDomainAppVirtualPath;
            if (!string.IsNullOrEmpty(appVirtualPath))
            {
                var webC = WebConfigurationManager.OpenWebConfiguration(appVirtualPath).ConnectionStrings;
                if (webC != null && webC.ConnectionStrings[name] != null)
                {
                    return webC.ConnectionStrings[name].ConnectionString;
                }
                return settings.GetNode(ConfigConstValues.SystemNodeName, name).Value;
            }
            else
            {
                return settings.GetNode(ConfigConstValues.SystemNodeName, name).Value;
            }
        }

        public static string GetConntectStringV2(this AppSettings settings, string connName)
        {
            var appVirtualPath = HttpRuntime.AppDomainAppVirtualPath;
            if (!string.IsNullOrEmpty(appVirtualPath))
            {
                var webC = WebConfigurationManager.OpenWebConfiguration(appVirtualPath).ConnectionStrings;
                if (webC != null && webC.ConnectionStrings[connName] != null)
                {
                    return webC.ConnectionStrings[connName].ConnectionString;
                }
                return ConnectionStringProvider.GetConnectionString(connName);
            }
            else
            {
                return ConnectionStringProvider.GetConnectionString(connName);
            }
        }

        public static string GetAppCode(this AppSettings settings)
        {
            return GetConfigSettings(settings, "appcode");
        }

        public static string GetApiAccess(this AppSettings settings)
        {
            var auth =  GetConfigSettings(settings, ConfigConstValues.ApiAccess);
            return string.IsNullOrEmpty(auth) ? "" : auth;
        }

        public static string GetAuthenticateProvider(this AppSettings settings)
        {
            var auth = GetConfigSettings(settings, ConfigConstValues.AuthenticateProvider);
            return string.IsNullOrEmpty(auth) ? "ApplicationCenter" : auth;
        }

        public static string GetSearchProvider(this AppSettings settings)
        {
            var auth = GetConfigSettings(settings, ConfigConstValues.SearchProvider);
            return string.IsNullOrEmpty(auth) ? "Base" : auth;
        }

        public static string GetAlias(this AppSettings settings)
        {
            var alias = GetConfigSettings(settings, ConfigConstValues.Alias);
            return string.IsNullOrEmpty(alias) ? "UnknowAlias" : alias;
        }

        public static string GetConfigSettings(this AppSettings settings, string code)
        {
            var appcode = System.Configuration.ConfigurationManager.AppSettings[code];
            return !string.IsNullOrEmpty(appcode) ? appcode : GetWebSettings(settings, code);
        }

        public static string GetAppVirtualDirectory(this AppSettings settings)
        {
            return GetConfigSettings(settings, ConfigConstValues.VirtualDirectory);
        }

        public static string GetGlobalization(this AppSettings settings)
        {
            if (_globalization != null)
                return _globalization;
            _globalization = _globalization ?? "zh-HK";
            var d= GetConfigSettings(settings, ConfigConstValues.Globalization);
            return d ?? _globalization;
        }

        public static string GetAppHost(this AppSettings settings)
        {
            return GetConfigSettings(settings, ConfigConstValues.Host);
        }

        private static string GetWebSettings(this AppSettings settings, string key)
        {
            var code = "";
            var appVirtualPath = HttpRuntime.AppDomainAppVirtualPath;
            if (!string.IsNullOrEmpty(appVirtualPath))
            {
                var webC = WebConfigurationManager.OpenWebConfiguration(appVirtualPath).AppSettings.Settings;
                if (webC.AllKeys.Contains(key))
                {
                    code = webC[key].Value;
                }
                else
                {
                    var node = settings.GetNode(ConfigConstValues.SystemNodeName, key);
                    code = node?.Value;
                }
            }
            else
            {
                var v = settings.GetNode(ConfigConstValues.SystemNodeName, key);
                if (v == null)
                {
                    return null;
                }
                code = settings.GetNode(ConfigConstValues.SystemNodeName, key).Value;
            }
            return code;
        }

        public static IList<SettingNode> GetNodesByParentNode(this AppSettings settings, string groupName, string parentName)
        {
            var nodes = settings.GetNode(groupName, parentName).Nodes;
            return nodes.Any() ? nodes : new List<SettingNode>();
        }

        public static SettingNode GetNodeByParentNode(this AppSettings settings, string groupName, string parentName,string key)
        {
            var list = GetNodesByParentNode(settings, groupName, parentName);
            if (list.Any(c => c.Key == key))
            {
                return list.FirstOrDefault(c => c.Key == key);
            }
            return new SettingNode();
        }

        public static bool GetEmailServiceState(this AppSettings settings)
        {
            var state = false;
            var data = GetConfigSettings(settings, ConfigConstValues.EnableEmailService);
            bool.TryParse(data, out state);
            return state;
        }

        public static string GetApplicationDisplayName(this AppSettings settings)
        {
            return GetConfigSettings(settings, ConfigConstValues.ApplicationDisplayName);
        }
        public static string GetNoAuthorizePageUrl(this AppSettings settings)
        {
            var appcode = GetAppVirtualDirectory(settings);
            var url = GetConfigSettings(settings, ConfigConstValues.NoAuthorizePage);
            url = $"/{appcode}/{url}";
            return url;
        }
        public static string GetSystemErrorPageUrl(this AppSettings settings)
        {
            var appcode = GetAppVirtualDirectory(settings);
            var url = GetConfigSettings(settings, ConfigConstValues.SystemErrorPage);
            url = $"/{appcode}/{url}";
            return url;
        }

        public static List<string> GetUrlWhiteList(this AppSettings settings)
        {
            if (_urlWhtieList != null && _urlWhtieList.Any()) return _urlWhtieList;
            _urlWhtieList = new List<string>();
            var nodes = settings.GetGroup(ConfigConstValues.WhiteList);
            nodes.Nodes.ForEach(c =>
            {
                if (c?.Value != null)
                {
                    var url = c.Value.ToLower();
                    if (!_urlWhtieList.Contains(url))
                        _urlWhtieList.Add(url);
                }

            });
            return _urlWhtieList;
        }

        public static bool IsInUrlWhiteList(this AppSettings settings, string url)
        {
            var list = GetUrlWhiteList(settings);
            var u = url.ToLower();
            return list.Any(s => u.Contains(s));
        }

        public static string GetUploadPath(this AppSettings settings)
        {
            return GetConfigSettings(settings, ConfigConstValues.UploadPath);
        }

        public static string GetDownloadLink(this AppSettings settings)
        {
            return GetConfigSettings(settings, ConfigConstValues.DownloadLink);
        }

        public static string GetExportPath(this AppSettings settings)
        {
            return GetConfigSettings(settings, ConfigConstValues.ExportPath);
        }

        public static string GetPackagePath(this AppSettings settings)
        {
            return GetConfigSettings(settings, ConfigConstValues.PackagePath);
        }

        public static string GetStorage(this AppSettings settings)
        {
            return GetConfigSettings(settings, ConfigConstValues.Storage);
        }

        public static string GetAuthenticateErrorRequestUrl(this AppSettings settings)
        {
            return GetConfigSettings(settings, ConfigConstValues.AuthenticateErrorRequestUrl);
        }

        public static string GetAuthenticateMode(this AppSettings settings)
        {
            if (_authenticateMode == null)
            {
                _authenticateMode = System.Configuration.ConfigurationManager.AppSettings["AuthenticateMode"];
                if (!string.IsNullOrEmpty(_authenticateMode))
                {
                    return _authenticateMode;
                }
                return GetWebSettings(settings, ConfigConstValues.AuthenticateMode);

            }
            return _authenticateMode;
        }

        public static string CachePersistenceMode(this AppSettings settings)
        {
            
            var re = GetConfigSettings(settings, ConfigConstValues.EnableCachePersistence); 
            if (re != null)
            {
                return re.ToUpper();
            }
            return "OFF";
        }

        public static void ClearCache()
        {
            _urlWhtieList = new List<string>();
            _authenticateMode = null;
            _globalization = null;
        }
    }
}
