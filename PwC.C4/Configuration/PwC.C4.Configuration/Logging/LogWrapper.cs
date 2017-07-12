using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using log4net;

namespace PwC.C4.Configuration.Logging
{
    internal class LogWrapper
    {
        static string GetConfigFile()
        {
            return LogConfigurationProvider.GetLogConfigurationFile();
        }

        static LogWrapper()
        {            
            string configFile = GetConfigFile();
            Reconfig(configFile);
            FileWatcher.Instance.AddFile(configFile, OnConfigChanged);
        }

        static void OnConfigChanged(object sender, EventArgs args)
        {
            string fileName = (string)sender;

            log4net.Util.LogLog.Warn(null, "Log configuration file '" + fileName + "' reload...");
            Reconfig(fileName);
        }

        static void Reconfig(string fileName)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);
                if (doc.DocumentElement != null)
                {
                    foreach (XmlElement elm in doc.DocumentElement.SelectNodes("appender/file"))
                    {
                        string logPath = elm.GetAttribute("value");
                        if (!string.IsNullOrEmpty(logPath))
                        {
                            elm.SetAttribute("value", Path.Combine(ConfigUtility.DefaultApplicationLogFolder, logPath));
                        }
                    }
                    log4net.Config.XmlConfigurator.Configure(doc.DocumentElement);
                }
            }
            catch (Exception ex)
            {
                log4net.Util.LogLog.Error(null,"XmlConfigurator: Failed to parse config file. Check your .config file is well formed XML.", ex);
            }

        }

        private ILog log;

        /// <summary>
        /// Creates a new instance of the logging wrapper by walking the stack to 
        /// find the calling class and configures the log based on this.
        /// </summary>
        public LogWrapper()
        {
            /*
             * Get the calling method, to determine the class name.
             * */
            StackFrame a = new StackFrame(1);
            MethodBase callingMethod = a.GetMethod();
            Type callingType = callingMethod.DeclaringType;
            log = LogManager.GetLogger(callingType.FullName);
        }

        public static log4net.ILog GetLogger(string name)
        {
            return LogManager.GetLogger(name);
        }

        public static log4net.ILog GetLoggerMore(string name)
        {
            return LogManager.GetLogger(name);
        }

        public bool IsDebugEnabled
        {
            get { return log.IsDebugEnabled; }
        }

        public bool IsErrorEnabled
        {
            get { return log.IsErrorEnabled; }
        }

        public bool IsInfoEnabled
        {
            get { return log.IsDebugEnabled; }
        }

        public bool IsWarnEnabled
        {
            get { return log.IsWarnEnabled; }
        }

        #region Debug
        public void Debug(object message, Exception exception)
        {
            log.Debug(message, exception);
        }

        public void Debug(object message)
        {
            log.Debug(message);
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            log.DebugFormat(provider, format, args);
        }

        public void DebugFormat(string format, params object[] args)
        {
            log.DebugFormat(format, args);
        }
        #endregion

        #region Error
        public void Error(object message, Exception exception)
        {
            log.Error(message, exception);
        }

        public void Error(object message)
        {
            log.Error(message);
        }

        public void Error(Exception exception)
        {
            log.Error(null, exception);
        }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            log.ErrorFormat(provider, format, args);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            log.ErrorFormat(format, args);
        }
        #endregion

        #region Info
        public void Info(object message, Exception exception)
        {
            log.Info(message, exception);
        }

        public void Info(object message)
        {
            log.Info(message);
        }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            log.InfoFormat(provider, format, args);
        }

        public void InfoFormat(string format, params object[] args)
        {
            log.InfoFormat(format, args);
        }
        #endregion

        #region Warn
        public void Warn(object message, Exception exception)
        {
            log.Warn(message, exception);
        }

        public void Warn(object message)
        {
            log.Warn(message);
        }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            log.WarnFormat(provider, format, args);
        }

        public void WarnFormat(string format, params object[] args)
        {
            log.WarnFormat(format, args);
        }
        #endregion

        #region Method Debug (Uses call-stack to output method name)
        /// <summary>
        /// Delegate to allow custom information to be logged
        /// </summary>
        /// <param name="logOutput">Initialized <see cref="StringBuilder"/> object which will be appended to output string</param>
        public delegate void LogOutputMapper(StringBuilder logOutput);

        public void MethodDebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            log.DebugFormat(provider, string.Format("Page: {2}, MethodName: {1}, {0}", format, GetDebugCallingMethod(), GetDebugCallingPage()), args);
        }

        public void MethodDebugFormat(string format, params object[] args)
        {
            log.DebugFormat(string.Format("Page: {2}, MethodName: {1}, {0}", format, GetDebugCallingMethod(), GetDebugCallingPage()), args);
        }

        public void MethodDebug(string message)
        {
            log.Debug(string.Format("Page: {2}, MethodName: {1}, {0}", message, GetDebugCallingMethod(), GetDebugCallingPage()));
        }

        // With Log Prefix

        public void MethodDebugFormat(IFormatProvider provider, string logPrefix, string format, params object[] args)
        {
            log.DebugFormat(provider, string.Format("{0}| {1} , MethodName: {2} , Page: {3}", logPrefix, format, GetDebugCallingMethod(), GetDebugCallingPage()), args);
        }

        public void MethodDebugFormat(string logPrefix, string format, params object[] args)
        {
            log.DebugFormat(string.Format("{0}| Page: {3}, MethodName: {2} , {1}", logPrefix, format, GetDebugCallingMethod(), GetDebugCallingPage()), args);
        }

        public void MethodDebug(string logPrefix, string message)
        {
            log.Debug(string.Format("{0}| Page: {3}, MethodName: {2}, {1}", logPrefix, message, GetDebugCallingMethod(), GetDebugCallingPage()));
        }

        // With Log Prefix and delegate to add custom logging info
        public void MethodDebugFormat(string logPrefix, LogOutputMapper customLogOutput, string format, params object[] args)
        {
            StringBuilder additionalLogData = new StringBuilder();
            if (customLogOutput != null)
                customLogOutput(additionalLogData);

            log.DebugFormat(string.Format("{0}| Page: {3}, MethodName: {2}, {1}, {4}", logPrefix, format, GetDebugCallingMethod(), GetDebugCallingPage(), additionalLogData.ToString()), args);
        }

        /// <summary>
        /// Returns calling method name using current stack 
        /// and assuming that first non Logging method is the parent
        /// </summary>
        /// <returns>Method Name</returns>
        private string GetDebugCallingMethod()
        {
            // Walk up the stack to get parent method
            StackTrace st = new StackTrace();
            if (st != null)
            {
                for (int i = 0; i < st.FrameCount; i++)
                {
                    StackFrame sf = st.GetFrame(i);
                    MethodBase method = sf.GetMethod();
                    if (method != null)
                    {
                        string delaringTypeName = method.DeclaringType.FullName;
                        if (delaringTypeName != null && delaringTypeName.IndexOf("PwC.C4.Logging") < 0)
                            return method.Name;
                    }
                }
            }

            return "Unknown Method";
        }

        public string CurrentStackTrace()
        {
            StringBuilder sb = new StringBuilder();
            // Walk up the stack to return everything
            StackTrace st = new StackTrace();
            if (st != null)
            {
                for (int i = 0; i < st.FrameCount; i++)
                {
                    StackFrame sf = st.GetFrame(i);
                    MethodBase method = sf.GetMethod();
                    if (method != null)
                    {
                        string declaringTypeName = method.DeclaringType.FullName;
                        if (declaringTypeName != null && declaringTypeName.IndexOf("PwC.C4.Logging") < 0)
                        {
                            sb.AppendFormat("{0}.{1}(", declaringTypeName, method.Name);

                            ParameterInfo[] paramArray = method.GetParameters();

                            if (paramArray.Length > 0)
                            {
                                for (int j = 0; j < paramArray.Length; j++)
                                {
                                    sb.AppendFormat("{0} {1}", paramArray[j].ParameterType.Name, paramArray[j].Name);
                                    if (j + 1 < paramArray.Length)
                                    {
                                        sb.Append(", ");
                                    }
                                }
                            }
                            sb.AppendFormat(")\n - {0}, {1}", sf.GetFileLineNumber(), sf.GetFileName());
                        }
                    }
                    else
                    {
                        sb.Append("The method returned null\n");
                    }
                }
            }
            else
            {
                sb.Append("Unable to get stack trace");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns ASP.NET method name which called current method. 
        /// Uses call stack and assumes that all methods starting with 'ASP.' are the ASP.NET page methods
        /// </summary>
        /// <returns>Class Name of the ASP.NET page</returns>
        private string GetDebugCallingPage()
        {
            // Walk up the stack to get calling method which is compiled ASP.Net page
            StackTrace st = new StackTrace();
            if (st != null)
            {
                for (int i = 0; i < st.FrameCount; i++)
                {
                    StackFrame sf = st.GetFrame(i);
                    MethodBase method = sf.GetMethod();
                    if (method != null && method.DeclaringType != null)
                    {
                        string declaringTypeName = method.DeclaringType.FullName;
                        if (declaringTypeName != null && declaringTypeName.IndexOf("ASP.") == 0)
                            return declaringTypeName;
                    }
                }
            }

            return "Unknown Page";
        }

        #endregion

        #region ILogMore methods

        public void MoreInfo(params object[] traceMessages)
        {
            log.Info(traceMessages);
        }

        public void MoreError(params object[] traceMessages)
        {
            log.Error(traceMessages);
        }

        public void MoreWarn(params object[] traceMessages)
        {
            log.Warn(traceMessages);
        }

        public void MoreDebug(params object[] traceMessages)
        {
            log.Debug(traceMessages);
        }

        public void MoreFatal(params object[] traceMessages)
        {
            log.Fatal(traceMessages);
        }

        public bool IsMoreDebugEnabled
        {
            get { return log.IsDebugEnabled; }
        }

        public bool IsMoreInfoEnabled
        {
            get { return log.IsInfoEnabled; }
        }

        public bool IsMoreErrorEnabled
        {
            get { return log.IsErrorEnabled; }
        }

        public bool IsMoreWarnEnabled
        {
            get { return log.IsWarnEnabled; }
        }

        public bool IsMoreFatalEnabled
        {
            get { return log.IsFatalEnabled; }
        }

        #endregion

        #region Exception Logging
        /// <summary>
        /// Logs exception 
        /// </summary>
        /// <param name="exc">Exception to log</param>
        /// <param name="policyName">Policy name to append to logged exception</param>
        /// <remarks>
        /// Does not rethrow exceptions. Use throw; statement to rethrow original exception within catch() block
        /// </remarks>
        /// <returns>true if successful</returns>
        public bool HandleException(Exception exc, string policyName)
        {
            log.Error(policyName, exc);
            return true;
        }
        #endregion
    }
}
