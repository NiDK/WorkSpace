using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using PwC.C4.Infrastructure.Config;
using PwC.C4.Infrastructure.Model.Enum;

namespace PwC.C4.Infrastructure.Logger
{
    public class LogWrapper
    {

        private ILog log;

        public LogWrapper(string type=null)
        {
            if (string.IsNullOrEmpty(type))
            {
                var a = new StackFrame(1);
                var callingMethod = a.GetMethod();
                var callingType = callingMethod.DeclaringType ?? callingMethod.ReflectedType;
                log = callingType == null ? LogManager.GetLogger("WCF Service") : LogManager.GetLogger(callingType);
            }
            else
            {
                log = LogManager.GetLogger(type);
            }
        }

        public void FixInfo(ErrorStatus errorStatus, Exception exception = null, string staffId = null)
        {
            try
            {
                if (exception != null) ThreadContext.Properties["Type"] = exception.GetType().FullName;
                if (staffId != null) ThreadContext.Properties["StaffId"] = staffId;
                ThreadContext.Properties["AppCode"] = AppSettings.Instance.GetAppCode();
                ThreadContext.Properties["Status"] = (int)errorStatus;
            }
            catch (Exception)
            {
                if (exception != null) ThreadContext.Properties["Type"] = "FixInfo Error"; ;
                if (staffId != null) ThreadContext.Properties["StaffId"] = "FixInfo Error"; ;
                ThreadContext.Properties["AppCode"] = "FixInfo Error";
                ThreadContext.Properties["Status"] = (int)errorStatus;
            }
            
        }

        public static ILog GetLogger(string name)
        {
            return LogManager.GetLogger(name);
        }


        private static void LoadAdoNetAppender()
        {
            try
            {
                var appender = new WcfAppender();
                BasicConfigurator.Configure(appender);
            }
            catch (Exception)
            {
                
            }
            
        }

        private static void LoadFileAppender()
        {
            try
            {
                var logPath = @"C:\PwC.C4.logfiles";

                var apps = AppSettings.Instance;

                var appCode = apps.GetAppCode();

                var alias = apps.GetAlias();

                logPath = Path.Combine(logPath, appCode);

                if (Directory.Exists(logPath) == false)
                {
                    Directory.CreateDirectory(logPath);
                }
                
                var txtLogPath = Path.Combine(logPath, appCode, alias + ".log");

                var hier =
                    LogManager.GetRepository() as Hierarchy;
                if (hier == null) return;

                var fileAppender = new RollingFileAppender
                {
                    Name = "LogFileAppender",
                    File = txtLogPath,
                    AppendToFile = true,
                    MaximumFileSize = "1MB",
                    MaxSizeRollBackups = 10,
                    StaticLogFileName = true
                };

                var patternLayout = new PatternLayout
                {
                    ConversionPattern =
                        "====================================================================%nDate:%date%nAppCode:%property{AppCode}%nException Type:%property{Type}%nStaff Id:%property{StaffId}%nThread:[%thread]%nLevel：%-5level%nClass：%logger%nMessage：%message%n"
                };
                patternLayout.ActivateOptions();
                fileAppender.Layout = patternLayout;
                fileAppender.Encoding = Encoding.UTF8;

                fileAppender.ActivateOptions();
                BasicConfigurator.Configure(fileAppender);
            }
            catch (Exception)
            {

            }
        }

        private static bool IsFileOpen(string filePath)
        {
            var result = false;
            try
            {
                var fs = File.OpenWrite(filePath);
                fs.Close();
            }
            catch (Exception ex)
            {
                result = true;
            }
            return result;
        }

        public static void InitLog()
        {
            try
            {
                var isEnbale = false;

                var enbale =
                    AppSettings.Instance.GetNode(ConfigConstValues.LoggerNodeName, ConfigConstValues.IsEnable).Value;

                bool.TryParse(enbale, out isEnbale);
                if (isEnbale)
                {
                    var logMethod =
                   AppSettings.Instance.GetNode(ConfigConstValues.LoggerNodeName, ConfigConstValues.LoggerSaveMethod).Value;
                    switch (logMethod.ToLower())
                    {
                        case "sql":
                            LoadAdoNetAppender();
                            break;
                        case "file":
                            LoadFileAppender();
                            break;
                        case "both":
                            LoadAdoNetAppender();
                            LoadFileAppender();
                            break;
                        default:
                            LoadFileAppender();
                            break;
                    }
                }
            }
            catch (Exception)
            {

            }
            
           
        }

         
        public bool IsDebugEnabled
        {
            get
            {
                try
                {
                    var result = false;
                    var status = AppSettings.Instance.GetNode(ConfigConstValues.LoggerNodeName, ConfigConstValues.IsEnableDebug).Value;
                    bool.TryParse(status, out result);
                    return result;
                }
                catch (Exception)
                {
                    return false;
                }
                
            }
        }

        public bool IsErrorEnabled
        {
            get { return log.IsErrorEnabled; }
        }

        public bool IsInfoEnabled
        {
            get { return this.IsDebugEnabled; }
        }

        public bool IsWarnEnabled
        {
            get { return log.IsWarnEnabled; }
        }

        #region Debug
        public void Debug(object message, Exception exception)
        {
            FixInfo(ErrorStatus.NeverRemind, exception);
            log.Debug(message, exception);
        }

        public void Debug(object message)
        {
            FixInfo(ErrorStatus.NeverRemind);
            log.Debug(message);
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            FixInfo(ErrorStatus.NeverRemind);
            log.DebugFormat(provider, format, args);
        }

        public void DebugFormat(string format, params object[] args)
        {
            FixInfo(ErrorStatus.NeverRemind);
            log.DebugFormat(format, args);
        }
        #endregion

        #region Error

        public void Error(object message)
        {
            FixInfo(ErrorStatus.Untreated);
            log.Error(message);
        }

        public void Error(Exception exception)
        {
            FixInfo(ErrorStatus.Untreated, exception);
            log.Error(null, exception);
        }

        public void Error(object message, Exception exception)
        {
            FixInfo(ErrorStatus.Untreated, exception);
            log.Error(message, exception);
        }

        public void Error(string staffId, object message, Exception exception)
        {
            FixInfo(ErrorStatus.Untreated, exception, staffId);
            log.Error(message, exception);
        }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            FixInfo(ErrorStatus.Untreated);
            log.ErrorFormat(provider, format, args);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            FixInfo(ErrorStatus.Untreated);
            log.ErrorFormat(format, args);
        }
        #endregion

        #region Info
        public void Info(object message, Exception exception)
        {
            FixInfo(ErrorStatus.NeverRemind, exception);
            log.Info(message, exception);
        }

        public void Info(object message)
        {
            FixInfo(ErrorStatus.NeverRemind);
            log.Info(message);
        }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            FixInfo(ErrorStatus.NeverRemind);
            log.InfoFormat(provider, format, args);
        }

        public void InfoFormat(string format, params object[] args)
        {
            FixInfo(ErrorStatus.NeverRemind);
            log.InfoFormat(format, args);
        }
        #endregion

        #region Warn
        public void Warn(object message, Exception exception)
        {
            FixInfo(ErrorStatus.NeverRemind, exception);
            log.Warn(message, exception);
        }

        public void Warn(object message)
        {
            FixInfo(ErrorStatus.NeverRemind);
            log.Warn(message);
        }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            FixInfo(ErrorStatus.NeverRemind);
            log.WarnFormat(provider, format, args);
        }

        public void WarnFormat(string format, params object[] args)
        {
            FixInfo(ErrorStatus.NeverRemind);
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
            var additionalLogData = new StringBuilder();
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
            var st = new StackTrace();
            for (int i = 0; i < st.FrameCount; i++)
            {
                var sf = st.GetFrame(i);
                var method = sf.GetMethod();
                if (method != null)
                {
                    string delaringTypeName = method.DeclaringType.FullName;
                    if (delaringTypeName != null && delaringTypeName.IndexOf("Infrastructure.Logger") < 0)
                        return method.Name;
                }
            }

            return "Unknown Method";
        }

        public string CurrentStackTrace()
        {
            var sb = new StringBuilder();
            // Walk up the stack to return everything
            var st = new StackTrace();
            for (int i = 0; i < st.FrameCount; i++)
            {
                var sf = st.GetFrame(i);
                var method = sf.GetMethod();
                if (method != null)
                {
                    string declaringTypeName = method.DeclaringType.FullName;
                    if (declaringTypeName != null && declaringTypeName.IndexOf("Infrastructure.Logger") < 0)
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
            for (int i = 0; i < st.FrameCount; i++)
            {
                var sf = st.GetFrame(i);
                var method = sf.GetMethod();
                if (method != null && method.DeclaringType != null)
                {
                    string declaringTypeName = method.DeclaringType.FullName;
                    if (declaringTypeName != null && declaringTypeName.IndexOf("ASP.") == 0)
                        return declaringTypeName;
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
            get { return this.IsDebugEnabled; }
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
