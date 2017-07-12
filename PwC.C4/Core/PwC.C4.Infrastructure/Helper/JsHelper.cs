namespace PwC.C4.Infrastructure.Helper
{
    public class JsHelper
    {
        public static string ExecuteJs(string strJs)
        {
            return "<script language=\"javascript\" type=\"text/javascript\">" + strJs + "</script>";
        }

        public static string Alert(string message)
        {
            return "<script language=\"javascript\" type=\"text/javascript\">$.weeboxs.notify('" + message + "', 'warning',5)</script>";
        }

        public static string Alert(string message, int width, int height)
        {
            return "<script language=\"javascript\" type=\"text/javascript\">$.weeboxs.open('" + message + "', { title: '提示', showCancel: false, width: " + width + ",height:" + height + " });</script>";
        }
    }
}
