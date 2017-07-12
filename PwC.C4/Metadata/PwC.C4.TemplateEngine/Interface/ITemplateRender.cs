using System.Web;

namespace PwC.C4.TemplateEngine.Interface
{
    public interface ITemplateRender
    {
        //public static IHtmlString CreateView(string template, Type type, dynamic model)
        //{
        //    var config = new TemplateServiceConfiguration
        //    {
        //        Language = Language.CSharp,
        //        BaseTemplateType = type,
        //        Namespaces = new HashSet<string>() {"TemplateEngine", "Service"}
        //    };

        //    var templateservice = new TemplateService(config);
        //    Razor.SetTemplateService(templateservice);

        //    string result = Razor.Parse(template, model);

        //    Console.WriteLine(result);
        //    Console.Read(); 
        //}

        IHtmlString CreatePreviewView(string template, dynamic model);
    }
}
