using System;
using System.IO;
using System.Text;

namespace PwC.C4.Configuration.Logging
{
    public class LogConfigurationProvider
    {
        private const string ConfigurationKeyForLoggingConfig = "LoggingConfigFile";
        private const string DefaultLoggingConfigFileName = "logging.production.config";
        private static string defaultLoggingConfigFilePath = Path.Combine(ConfigUtility.DefaultApplicationConfigFolder, DefaultLoggingConfigFileName);

        #region DefaultLogConfiguration
        private static string default_trace = Path.Combine(ConfigUtility.DefaultApplicationLogFolder, "trace.log");
        private static string default_trace_config = Path.Combine(ConfigUtility.DefaultApplicationLogFolder, @"trace_config.log");
        private static string default_trace_filenotexist = Path.Combine(ConfigUtility.DefaultApplicationLogFolder, @"trace_filenotexist.log");

        private static string default_trace_security = Path.Combine(ConfigUtility.DefaultApplicationLogFolder, @"trace_security.log");
        public const string FilenotexistLogName = "PwC.C4.FileNotExistLog";
        public const string SecurityLogName = "PwC.C4.SecurityLog";

        private static string default_trace_rollminute = Path.Combine(ConfigUtility.DefaultApplicationLogFolder, @"trace_rollminute.log");
        public const string RollminuteLogName = "PwC.C4.RollminuteLog";

        private static string default_trace_perf = Path.Combine(ConfigUtility.DefaultApplicationLogFolder, "trace_perf.log");
        public const string PerfLoggerName = "PwC.C4.Tracing.RaytraceLogger";

        private static string GetDefaultLogConfigurationContent()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<log4net>");
            builder.AppendLine(@"<appender name=""RollingFileAppender"" type=""log4net.Appender.RollingFileAppender"">");
            builder.AppendLine(@"<file value=" + "\"" + default_trace + "\"" + "/>");
            builder.AppendLine(@"<appendToFile value=""true"" />");
            builder.AppendLine(@"<rollingStyle value=""Size"" />");
            builder.AppendLine(@"<maxSizeRollBackups value=""10"" />");
            builder.AppendLine(@"<maximumFileSize value=""1000KB"" />");
            builder.AppendLine(@"<staticLogFileName value=""true"" />");
            builder.AppendLine(@"<layout type=""log4net.Layout.PatternLayout"">");
            builder.AppendLine(@"<conversionPattern value=""%d [%t] %-5p %c [%x] - %m%n"" />");
            builder.AppendLine(@"</layout>");
            builder.AppendLine(@"<filter type=""log4net.Filter.LoggerMatchFilter"">");
            builder.AppendLine(@"<loggerToMatch value=""PwC.C4.Configuration.BaseConfigurationManager"" />");
            builder.AppendLine(@"<acceptOnMatch value=""false"" />");
            builder.AppendLine(@"</filter>");
            builder.AppendLine(@"<filter type=""log4net.Filter.LoggerMatchFilter"">");
            builder.AppendLine(@"<loggerToMatch value=""PwC.C4.Configuration.DirectoryWatcher"" />");
            builder.AppendLine(@"<acceptOnMatch value=""false"" />");
            builder.AppendLine(@"</filter>");

            //add FilenotExist Filter Start
            builder.AppendLine(@"<filter type=""log4net.Filter.LoggerMatchFilter"">");
            builder.AppendLine(@"<loggerToMatch value=" + "\"" + FilenotexistLogName + "\"" + "/>");
            builder.AppendLine(@"<acceptOnMatch value=""false"" />");
            builder.AppendLine(@"</filter>");
            //add FilenotExist Filter End            

            //add Security Filter Start
            builder.AppendLine(@"<filter type=""log4net.Filter.LoggerMatchFilter"">");
            builder.AppendLine(@"<loggerToMatch value=" + "\"" + SecurityLogName + "\"" + "/>");
            builder.AppendLine(@"<acceptOnMatch value=""false"" />");
            builder.AppendLine(@"</filter>");
            //add Security Filter End           

            //add RollminuteLog Start
            builder.AppendLine(@"<filter type=""log4net.Filter.LoggerMatchFilter"">");
            builder.AppendLine(@"<loggerToMatch value=" + "\"" + RollminuteLogName + "\"" + "/>");
            builder.AppendLine(@"<acceptOnMatch value=""false"" />");
            builder.AppendLine(@"</filter>");
            //add RollminuteLog End

            builder.AppendLine(@"<filter type=""log4net.Filter.LoggerMatchFilter"">");
            builder.AppendLine(@"<loggerToMatch value=" + "\"" + PerfLoggerName + "\"" + "/>");
            builder.AppendLine(@"<acceptOnMatch value=""false"" />");
            builder.AppendLine(@"</filter>");

            builder.AppendLine(@"</appender>");
            builder.AppendLine();

            builder.AppendLine(@"<appender name=""RollingFileAppenderConfiguration"" type=""log4net.Appender.RollingFileAppender"">");
            builder.AppendLine(@"<file value='" + default_trace_config + "' />");
            builder.AppendLine(@"<appendToFile value=""true"" />");
            builder.AppendLine(@"<rollingStyle value=""Size"" />");
            builder.AppendLine(@"<maxSizeRollBackups value=""20"" />");
            builder.AppendLine(@"<maximumFileSize value=""3000KB"" />");
            builder.AppendLine(@"<layout type=""log4net.Layout.PatternLayout"">");
            builder.AppendLine(@"<conversionPattern value=""%d [%t] %-5p %c [%x] - %m%n"" />");
            builder.AppendLine(@"</layout>");
            builder.AppendLine(@"<filter type=""log4net.Filter.LoggerMatchFilter"">");
            builder.AppendLine(@"<loggerToMatch value=""PwC.C4.Configuration.BaseConfigurationManager"" />");
            builder.AppendLine(@"<acceptOnMatch value=""true"" />");
            builder.AppendLine(@"</filter>");
            builder.AppendLine(@"<filter type=""log4net.Filter.LoggerMatchFilter"">");
            builder.AppendLine(@"<loggerToMatch value=""PwC.C4.Configuration.DirectoryWatcher"" />");
            builder.AppendLine(@"<acceptOnMatch value=""true"" />");
            builder.AppendLine(@"</filter>");
            builder.AppendLine(@"<filter type=""log4net.Filter.DenyAllFilter"" />");
            builder.AppendLine(@"</appender>");
            builder.AppendLine();

            builder.AppendLine(@"<appender name=""RollingFileAppenderTracing"" type=""log4net.Appender.RollingFileAppender"">");
            builder.AppendLine(@"<file value='" + default_trace_perf + "' />");
            builder.AppendLine(@"<appendToFile value=""true"" />");
            builder.AppendLine(@"<rollingStyle value=""Size"" />");
            builder.AppendLine(@"<maxSizeRollBackups value=""30"" />");
            builder.AppendLine(@"<maximumFileSize value=""3000KB"" />");
            builder.AppendLine(@"<layout type=""log4net.Layout.PatternLayout"">");
            builder.AppendLine(@"<conversionPattern value=""%m%n"" />");
            builder.AppendLine(@"</layout>");
            builder.AppendLine(@"<filter type=""log4net.Filter.LoggerMatchFilter"">");
            builder.AppendLine(@"<loggerToMatch value=""PwC.C4.Tracing.RaytraceLogger"" />");
            builder.AppendLine(@"<acceptOnMatch value=""true"" />");
            builder.AppendLine(@"</filter>");
            builder.AppendLine(@"<filter type=""log4net.Filter.DenyAllFilter"" />");
            builder.AppendLine(@"</appender>");
            builder.AppendLine();

            //add FilenotExist Filter Start
            builder.AppendLine(@"<appender name=""RollingFileAppenderFileNotExist"" type=""log4net.Appender.RollingFileAppender"">");
            builder.AppendLine(@"<file value=" + "\"" + default_trace_filenotexist + "\"" + "/>");
            builder.AppendLine(@"<appendToFile value=""true"" />");
            builder.AppendLine(@"<rollingStyle value=""Size"" />");
            builder.AppendLine(@"<maxSizeRollBackups value=""10"" />");
            builder.AppendLine(@"<maximumFileSize value=""1000KB"" />");
            builder.AppendLine(@"<staticLogFileName value=""true"" />");
            builder.AppendLine(@"<layout type=""log4net.Layout.PatternLayout"">");
            builder.AppendLine(@"<conversionPattern value=""%d [%t] %-5p %c [%x] - %m%n"" />");
            builder.AppendLine(@"</layout>");
            builder.AppendLine(@"<filter type=""log4net.Filter.LoggerMatchFilter"">");
            builder.AppendLine(@"<loggerToMatch value=" + "\"" + FilenotexistLogName + "\"" + "/>");
            builder.AppendLine(@"<acceptOnMatch value=""true"" />");
            builder.AppendLine(@"</filter>");
            builder.AppendLine(@"<filter type=""log4net.Filter.DenyAllFilter"" />");
            builder.AppendLine(@"</appender>");
            builder.AppendLine();
            //add FilenotExist Filter End           

            //add Security Filter Start
            builder.AppendLine(@"<appender name=""RollingFileAppenderSecurity"" type=""log4net.Appender.RollingFileAppender"">");
            builder.AppendLine(@"<file value=" + "\"" + default_trace_security + "\"" + "/>");
            builder.AppendLine(@"<appendToFile value=""true"" />");
            builder.AppendLine(@"<rollingStyle value=""Size"" />");
            builder.AppendLine(@"<maxSizeRollBackups value=""10"" />");
            builder.AppendLine(@"<maximumFileSize value=""1000KB"" />");
            builder.AppendLine(@"<staticLogFileName value=""true"" />");
            builder.AppendLine(@"<layout type=""log4net.Layout.PatternLayout"">");
            builder.AppendLine(@"<conversionPattern value=""%d [%t] %-5p %c [%x] - %m%n"" />");
            builder.AppendLine(@"</layout>");
            builder.AppendLine(@"<filter type=""log4net.Filter.LoggerMatchFilter"">");
            builder.AppendLine(@"<loggerToMatch value=" + "\"" + SecurityLogName + "\"" + "/>");
            builder.AppendLine(@"<acceptOnMatch value=""true"" />");
            builder.AppendLine(@"</filter>");
            builder.AppendLine(@"<filter type=""log4net.Filter.DenyAllFilter"" />");
            builder.AppendLine(@"</appender>");
            builder.AppendLine();
            //add Security Filter End  

            //add Rollminute Start RollingStyle is  Date,yyyyMMdd-HHmm,per minute
            builder.AppendLine(@"<appender name=""RollingFileAppenderRollminute"" type=""log4net.Appender.RollingFileAppender"">");
            builder.AppendLine(@"<file value=" + "\"" + default_trace_rollminute + "\"" + "/>");
            builder.AppendLine(@"<appendToFile value=""true"" />");
            builder.AppendLine(@"<rollingStyle value=""Date"" />");
            builder.AppendLine(@"<datePattern value=""yyyyMMdd-HHmm"" />");
            builder.AppendLine(@"<staticLogFileName value=""true"" />");
            builder.AppendLine(@"<layout type=""log4net.Layout.PatternLayout"">");
            builder.AppendLine(@"<conversionPattern value=""%m%n"" />");
            builder.AppendLine(@"</layout>");
            builder.AppendLine(@"<filter type=""log4net.Filter.LoggerMatchFilter"">");
            builder.AppendLine(@"<loggerToMatch value=" + "\"" + RollminuteLogName + "\"" + "/>");
            builder.AppendLine(@"<acceptOnMatch value=""true"" />");
            builder.AppendLine(@"</filter>");
            builder.AppendLine(@"<filter type=""log4net.Filter.DenyAllFilter"" />");
            builder.AppendLine(@"</appender>");
            builder.AppendLine();
            //add Rollminute End

            builder.AppendLine(@"<appender name=""ErrorTrackerAppender"" type=""PwC.C4.ErrorTracker.Client.ETLogAppender,PwC.C4.ErrorTracker.Client"">");
            builder.AppendLine(@"<filter type=""log4net.Filter.LoggerMatchFilter"">");
            builder.AppendLine(@"<loggerToMatch value=""PwC.C4.Tracing.RaytraceLogger""/>");
            builder.AppendLine(@"<acceptOnMatch value=""false"" />");
            builder.AppendLine(@"</filter>");
            builder.AppendLine(@"<filter type=""log4net.Filter.LevelRangeFilter"">");
            builder.AppendLine(@"<param name=""LevelMin"" value=""ERROR"" />");
            builder.AppendLine(@"<param name=""LevelMax"" value=""FATAL"" />");
            builder.AppendLine(@"</filter>");
            builder.AppendLine(@"<filter type=""log4net.Filter.DenyAllFilter"" />");
            builder.AppendLine(@"</appender>");
            builder.AppendLine();

            builder.AppendLine(@"<root>");
            builder.AppendLine(@"<level value=""ERROR"" />");
            builder.AppendLine(@"<appender-ref ref=""ErrorTrackerAppender"" />");
            builder.AppendLine(@"<appender-ref ref=""RollingFileAppender"" />");
            builder.AppendLine(@"<appender-ref ref=""RollingFileAppenderConfiguration"" />");
            builder.AppendLine(@"<appender-ref ref=""RollingFileAppenderTracing"" />");
            builder.AppendLine(@"<appender-ref ref=""RollingFileAppenderFileNotExist"" />");
            builder.AppendLine(@"<appender-ref ref=""RollingFileAppenderSecurity"" />");

            //default is off            
            // builder.AppendLine(@"<appender-ref ref=""RollingFileAppenderRollminute"" />");
            builder.AppendLine(@"</root>");
            builder.AppendLine(@"</log4net>");
            return builder.ToString();
        }

        #endregion

        public static string GetLogConfigurationFile()
        {
            //remove default logging.production.config logic
            string configFileValue = System.Configuration.ConfigurationManager.AppSettings[ConfigurationKeyForLoggingConfig];
            //if (string.IsNullOrEmpty(configFileValue))
            //    configFileValue = DefaultLoggingConfigFileName;
            if (!string.IsNullOrEmpty(configFileValue))
            {
                configFileValue = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFileValue);
                if (File.Exists(configFileValue))
                    return configFileValue;
            }

            if (File.Exists(defaultLoggingConfigFilePath))
                return defaultLoggingConfigFilePath;
            else
                return GenerateDefaultLogConfigurationFile();
        }

        private static string GenerateDefaultLogConfigurationFile()
        {
            try
            {
                if (!Directory.Exists(ConfigUtility.DefaultApplicationConfigFolder))
                {
                    Directory.CreateDirectory(ConfigUtility.DefaultApplicationConfigFolder);
                }

                File.WriteAllText(defaultLoggingConfigFilePath, GetDefaultLogConfigurationContent());
                return defaultLoggingConfigFilePath;
            }
            catch (IOException)
            {
                return null;
            }
        }
    }
}
