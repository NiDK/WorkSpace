using PwC.C4.TemplateEngine.Common;

namespace PwC.C4.TemplateEngine.Extensions
{
    public class RenderHelper
    {
        public dynamic ToDynamic(string modelJson, string name)
        {
            return DynamicJsonConverter.Parse(modelJson);
        }
    }
}
