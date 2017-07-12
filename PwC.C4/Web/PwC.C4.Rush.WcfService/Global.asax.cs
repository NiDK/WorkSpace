using System.Web.Routing;

namespace PwC.C4.Rush.WcfService
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            PwC.C4.Infrastructure.BaseLogger.LogWrapper.InitLog();
        }
    }
}
