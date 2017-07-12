using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Xml.Serialization;
using log4net;
using PwC.C4.Configuration.Messager.Service;
using PwC.C4.Configuration.Messager.Service.ServiceImp;
using PwC.C4.Infrastructure.Logger;

namespace PwC.C4.Configuration.Messager
{
    public class Global : System.Web.HttpApplication
    {
        PwC.C4.Infrastructure.Logger.LogWrapper log = new LogWrapper();
        //System.Diagnostics.PerformanceCounter ConfigVersionCounter = null;
        //System.Diagnostics.PerformanceCounter ResourceMgrCounter = null;
        //System.Diagnostics.PerformanceCounter ConfigMgrCounter = null;
        private SqlDependencyNotification notfication = null;
        #region readonly string
        private readonly string REMOTE_CONFIGURATION = "C4_RemoteConfiguration";
        private readonly string CONFIG_VERSION_COUNTER = "configVersionCounter";
        private readonly string RESOURCE_MGR_COUNTER = "resourceMgrCounter";
        private readonly string CONFIG_MGR_COUNTER = "configMgrCounter";
        #endregion

        public Global()
        {
            notfication = new SqlDependencyNotification();
            //if (!PerformanceCounterCategory.Exists(REMOTE_CONFIGURATION))
            //{
            //   // CreatePerformanceCounter();
            //}
            //ConfigVersionCounter = new PerformanceCounter(REMOTE_CONFIGURATION, CONFIG_VERSION_COUNTER, false);
            //ResourceMgrCounter = new PerformanceCounter(REMOTE_CONFIGURATION, RESOURCE_MGR_COUNTER, false);
            //ConfigMgrCounter = new PerformanceCounter(REMOTE_CONFIGURATION, CONFIG_MGR_COUNTER, false);

            //ConfigVersionCounter.RawValue = 0;
            //ResourceMgrCounter.RawValue = 0;
            //ConfigMgrCounter.RawValue = 0;
        }

        #region protected members
        protected void Application_Start(object sender, EventArgs e)
        {
            LogWrapper.InitLog();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            notfication.RegisterDependencyUsingDefaultQueue();
            ConfigurationService.Instance().InitConfigurationByNotification();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            string strCurrentPath = Request.Path.ToLower();
            //if (strCurrentPath.IndexOf("configversionhandler.ashx") >= 0)
            //    ConfigVersionCounter.Increment();
            //else if (strCurrentPath.IndexOf("resourcemanagerhandler.ashx") >= 0)
            //    ResourceMgrCounter.Increment();
            //else if (strCurrentPath.IndexOf("configmanagerhandler.ashx") >= 0)
            //    ConfigMgrCounter.Increment();

            if (bool.Parse(ConfigurationManager.AppSettings["debug"]))
            {
                string Info = string.Empty;
                if (strCurrentPath != null)
                {
                    Info = string.Format("Begin Request: {0}", strCurrentPath);

                    try
                    {
                        if (strCurrentPath.ToLower().IndexOf("configversionhandler.ashx") >= 0)
                        {
                            XmlSerializer xser = new XmlSerializer(typeof(RemoteConfigSectionCollection));
                            RemoteConfigSectionCollection rcc = (RemoteConfigSectionCollection)xser.Deserialize(Request.InputStream);
                            Request.InputStream.Seek(0, SeekOrigin.Begin);
                            Info += string.Format("\r\n\tApplication:{0} Machine:{1}", rcc.Application, rcc.Machine);
                        }
                        else if ((strCurrentPath.ToLower().IndexOf("resourcemanagerhandler.ashx") >= 0) || (strCurrentPath.ToLower().IndexOf("configmanagerhandler.ashx") >= 0))
                        {
                            XmlSerializer xser = new XmlSerializer(typeof(RemoteConfigManagerDTO));
                            RemoteConfigManagerDTO rcm = (RemoteConfigManagerDTO)xser.Deserialize(Request.InputStream);
                            Request.InputStream.Seek(0, SeekOrigin.Begin);
                            Info += string.Format("\r\n\tCommand:{0} OperatorID:{1} Application:{2} Machine:{3}", rcm.Operation.Command, rcm.Operation.OperatorID, rcm.RemoteConfigSections.Application, rcm.RemoteConfigSections.Machine);
                        }
                    }
                    catch (Exception err)
                    {
                        if (Request.InputStream != null)
                        {
                            Request.InputStream.Seek(0, SeekOrigin.Begin);
                            byte[] bytes = new byte[Request.InputStream.Length + 1];
                            Request.InputStream.Read(bytes, 0, (int)(Request.InputStream.Length));
                            string streamContent = new UTF8Encoding().GetString(bytes);
                            Request.InputStream.Seek(0, SeekOrigin.Begin);

                            log.Error(streamContent, err);
                        }
                    }

                    log.Info(Info);
                }
            }
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            if (bool.Parse(ConfigurationManager.AppSettings["debug"]))
            {
                string strCurrentPath = Request.Path;
                if (strCurrentPath != null)
                    log.Info(string.Format("End Request: {0}", strCurrentPath));
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

            var ex = Server.GetLastError();

            if (ex != null)
            {
                log.Error("Application error", new Exception($"ErrorPath:{Request.Path}", ex));
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            notfication.Dispose();

            log.Info("Web ending...");
        }

        protected void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception exc = e.ExceptionObject as Exception;

            if (exc != null)
            {
                log.Error("Unhandled exception on thread", exc);
            }
        }
        #endregion

        #region private members
        private void CreatePerformanceCounter()
        {
            CounterCreationDataCollection counters = new CounterCreationDataCollection();

            CounterCreationData configVersionPerSecond = new CounterCreationData();
            configVersionPerSecond.CounterName = CONFIG_VERSION_COUNTER;
            configVersionPerSecond.CounterType = PerformanceCounterType.RateOfCountsPerSecond32;
            configVersionPerSecond.CounterHelp = "configuration version response count per second";
            counters.Add(configVersionPerSecond);

            CounterCreationData resourceMgrPerSecond = new CounterCreationData();
            resourceMgrPerSecond.CounterName = RESOURCE_MGR_COUNTER;
            resourceMgrPerSecond.CounterType = PerformanceCounterType.RateOfCountsPerSecond32;
            resourceMgrPerSecond.CounterHelp = "resource manager response count per second";
            counters.Add(resourceMgrPerSecond);

            CounterCreationData configMgrPerSecond = new CounterCreationData();
            configMgrPerSecond.CounterName = CONFIG_MGR_COUNTER;
            configMgrPerSecond.CounterType = PerformanceCounterType.RateOfCountsPerSecond32;
            configMgrPerSecond.CounterHelp = "configuration management response count per second";
            counters.Add(configMgrPerSecond);

            PerformanceCounterCategory.Create(REMOTE_CONFIGURATION,
                              "Remote Configuration Management Counter",
                              PerformanceCounterCategoryType.SingleInstance, counters);
        }
        #endregion
    }
}