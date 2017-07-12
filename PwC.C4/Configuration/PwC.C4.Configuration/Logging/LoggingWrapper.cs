using System;
using System.Diagnostics;
using log4net;

namespace PwC.C4.Configuration.Logging
{
    /// <summary>
    /// Summary description for LoggingWrapper.
    /// </summary>
    public class LoggingWrapper
    {
        public LoggingWrapper()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private static LogWrapper AppLog = new LogWrapper();


        public static log4net.ILog GetLogger(string name)
        {
            return LogWrapper.GetLogger(name);
        }

        public static log4net.ILog GetLoggerMore(string name)
        {
            return LogWrapper.GetLoggerMore(name);
        }

        static LoggingWrapper()
        {

        }
        public static void Write(string message, string category)
        {

            AppLog.Warn(category + ":" + message);
        }

        public static void Write(string message)
        {
            AppLog.Warn(message);
        }

        public static bool HandleException(Exception exc, string policyName)
        {
            AppLog.Error(policyName, exc);
            return true;
        }


        public static void WarnFileNotExist(string message)
        {
            GetLogger(LogConfigurationProvider.FilenotexistLogName).Warn(message);
        }

        public static void WarnFileNotExist(object message, Exception exception)
        {
            GetLogger(LogConfigurationProvider.FilenotexistLogName).Warn(message, exception);
        }        

        public static void WarnSecurity(string message)
        {
            GetLogger(LogConfigurationProvider.SecurityLogName).Warn(message);
        }

        public static void WarnSecurity(object message, Exception exception)
        {
            GetLogger(LogConfigurationProvider.SecurityLogName).Warn(message, exception);
        }        

        #region EVENT LOG METHODS

        /// <summary>
        /// Write to error message to event log (uses Log4Net)
        /// 
        /// Used when application explicitly throws a custom exception with no 
        /// stack trace associated with the the error message that needs to be recorded
        /// </summary>
        /// <param name="loggerName"></param>
        /// <param name="message"></param>
        /// <param name="category"></param>
        public static void WriteToEventLog(string loggerName, string message, string category)
        {
            // Set to event logger type (e.g., warning or error message type) specified in app.config
            ILog logWriter = LogManager.GetLogger(loggerName);

            logWriter.Error(category + ":" + message);
        }


        /// <summary>
        /// Write fatal exception with stack trace to event log (uses Log4Net)
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="loggerName"></param>
        public static void WriteToEventLog(System.Exception ex, string loggerName)
        {
            // Set to event logger type (e.g., warning or error message type) specified in app.config
            ILog logWriter = LogManager.GetLogger(loggerName);

            logWriter.Fatal(ex);
        }


        /// <summary>
        /// Write to event log using System.Diagnostics
        /// Note: this method can insert specfic event and category ID information 
        /// if MOM requires this info for detail monitoring and report generation
        /// 
        /// </summary>
        /// <param name="eventID"></param>
        /// <param name="eventCategory"></param>
        /// <param name="source"></param>
        /// <param name="logName"></param>
        /// <param name="message"></param>
        public static void WriteToEventLog(short eventID, short eventCategory, string source, string logName, string message)
        {
            string eventMessage;

            if (!EventLog.SourceExists(logName))
            {
                EventLog.CreateEventSource(source, logName);
            }

            // Limit event log message to 32K
            if (message.Length > 32000)
            {
                // Truncate excess characters
                eventMessage = message.Substring(0, 32000);
            }
            else
            {
                eventMessage = message;
            }

            EventLog.WriteEntry(source, eventMessage, EventLogEntryType.Error, eventID, eventCategory);
        }


        public static void LogErrorToDatabase(string fuseAction, string serverName, string userAgent, string errorText, string formData, string urlData, string cookieData, int userId, string serverIp)
        {
            throw new Exception("Not Support!");           
        }


        #endregion

    }
}
