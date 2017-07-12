using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PwC.C4.TemplateEngine.Extensions
{
    public class RushRenderHelper
    {

        public MvcHtmlString Input(string type,string name, object value, object attr)
        {
            var tesb = new StringBuilder();
            tesb.Append("<input type=\"" + type + "\" name=\"" + name + "\" value=\"" + value + "\"");

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(attr))
            {
                tesb.Append(property.Name.Replace('_', '-') + "=\"" + property.GetValue(attr) + "\" ");
            }
            tesb.Append("/>");
            return new MvcHtmlString(tesb.ToString());

        }

        public MvcHtmlString TextBox(string name, object value, object attr)
        {
            var tesb = new StringBuilder();
            tesb.Append("<input type=\"text\" name=\"" + name + "\" value=\"" + value + "\"");

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(attr))
            {
                tesb.Append(property.Name.Replace('_', '-') + "=\"" + property.GetValue(attr) + "\" ");
            }
            tesb.Append("/>");
            return new MvcHtmlString(tesb.ToString());

        }

        public MvcHtmlString HiddenId(object value, object attr)
        {
            var tesb = new StringBuilder();
            tesb.Append("<input type=\"hidden\" id=\"KeyIdForRender\" name=\"KeyIdForRender\" value=\"" + value + "\"");

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(attr))
            {
                tesb.Append(property.Name.Replace('_', '-') + "=\"" + property.GetValue(attr) + "\" ");
            }
            tesb.Append("/>");
            return new MvcHtmlString(tesb.ToString());

        }

        public MvcHtmlString FormBegin(string name, object attr)
        {
            var form = new StringBuilder();
            form.Append("<form id=\"" + name + "\" name=\"" + name + "\"");
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(attr))
            {
                form.Append(property.Name.Replace('_', '-') + "=\"" + property.GetValue(attr) + "\" ");
            }
            form.Append(">");
            return new MvcHtmlString(form.ToString());
        }

        public MvcHtmlString FormEnd()
        {
            return new MvcHtmlString("</form>");
        }

        public IHtmlString Raw(string value)
        {
            return new HtmlString(value);
        }

        public MvcHtmlString SubmitBtn(string value, object attr, string formName)
        {
            var btn = new StringBuilder();
            btn.Append("<button type=\"button\" id=\"SubmitBtnForRender\" name=\"SubmitBtnForRender\" formName=\""+ formName +"\" value=\"" + value + "\"");
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(attr))
            {
                btn.Append(property.Name.Replace('_', '-') + "=\"" + property.GetValue(attr) + "\" ");
            }
            btn.Append(">" + value + "</button>");
            return new MvcHtmlString(btn.ToString());
        }

    }
}
