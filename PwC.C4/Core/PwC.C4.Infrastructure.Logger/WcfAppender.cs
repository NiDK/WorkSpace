using System.Collections.Generic;
using System.Linq;
using log4net;
using log4net.Appender;
using log4net.Core;
using PwC.C4.Infrastructure.Helper;

namespace PwC.C4.Infrastructure.Logger
{
    public class WcfAppender : AppenderSkeleton
    {
        static C4LogServiceClient _client;

        public WcfAppender()
        {

            CreateChannelToWcfService();
        }

        private static void CreateChannelToWcfService()
        {
            _client = new C4LogServiceClient();
        }

        #region AppenderSkeleton Members

        protected override void Append(LoggingEvent loggingEvent)
        {

            var status = 0;
            var appcode = "";
            var type = "";
            var staffId = "";
            var execption = "";
            var dic = ThreadContext.Properties.GetKeys().ToList();
            if (dic.Contains("Status"))
            {
                int.TryParse(ThreadContext.Properties["Status"].ToString(), out status);
            }
            if (dic.Contains("AppCode"))
            {
                appcode = ThreadContext.Properties["AppCode"].ToString();
            }
            if (dic.Contains("Type"))
            {
                type = ThreadContext.Properties["Type"].ToString();
            }
            if (dic.Contains("StaffId"))
            {
                staffId = ThreadContext.Properties["StaffId"].ToString();
            }
            if (loggingEvent.ExceptionObject != null)
            {
                execption = loggingEvent.ExceptionObject.ToString();
            }

            _client.Log_ForException_Insert(
                appcode,
                type,
                loggingEvent.TimeStamp,
                staffId,
                loggingEvent.ThreadName,
                loggingEvent.Level.Name,
                loggingEvent.RenderedMessage,
                execption,
                loggingEvent.LoggerName,
                status);
        }

        #endregion
    }
}
