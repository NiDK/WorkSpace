using System.Web;
using PwC.C4.TemplateEngine.Interface;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace PwC.C4.TemplateEngine.ServiceImp
{
    public class TemplateRender : ITemplateRender
    {
        private readonly ITemplateService _service;

        public TemplateRender()
        {
            this._service = new TemplateService(new TemplateServiceConfiguration()
            {
                BaseTemplateType = typeof(global::PwC.C4.TemplateEngine.HtmlTemplateBase<>)
            });
        }

        public IHtmlString CreatePreviewView(string template, dynamic model)
        {
            return new HtmlString(_service.Parse(template, model, null, null));
        }
    }
}
