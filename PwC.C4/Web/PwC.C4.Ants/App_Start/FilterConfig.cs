using System.Web;
using System.Web.Mvc;

namespace PwC.C4.Ants
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
