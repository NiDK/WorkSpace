using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace PwC.C4.Infrastructure.WebExtension
{

    ///<summary>
    ///</summary>
    public class JsonpViewResult : ViewResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            var request = context.HttpContext.Request;
            var response = context.HttpContext.Response;

            var callbackFunc = request.QueryString["callback"];
            if (!string.IsNullOrEmpty(callbackFunc))
            {

                if (String.IsNullOrEmpty(ViewName))
                {
                    ViewName = context.RouteData.GetRequiredString("action");
                }

                ViewEngineResult result = null;

                if (View == null)
                {
                    result = FindView(context);
                    View = result.View;
                }


                var sb = new StringBuilder(1024);

                using (TextWriter writer = new StringWriter(sb))
                {
                    var viewContext = new ViewContext(context, View, ViewData, TempData, writer);
                    View.Render(viewContext, writer);
                    writer.Flush();
                }


                var content = JsonConvert.SerializeObject(sb.ToString());
                response.ContentType = "application/json";

                response.Write(callbackFunc);
                response.Write('(');

                response.Write(content);
                response.Write(')');

                if (result != null)
                {
                    result.ViewEngine.ReleaseView(context, View);
                }
            }
            else
            {
                base.ExecuteResult(context);
            }

        }
    }

}