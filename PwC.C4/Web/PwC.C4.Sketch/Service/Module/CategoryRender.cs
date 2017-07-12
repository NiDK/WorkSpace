using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using PwC.C4.DataService.Model;

namespace PwC.C4.Sketch.Service.Module
{
    public static class CategoryRender
    {
        public static string GetCategory(List<HtmlCategory> categories)
        {
            var html = new StringBuilder();
            html.Append("<ul class=\"box\">");
            for (int n = 0; n < categories.Count(); n++)
            {
                var c = categories[n];
                var expStyle1 = "collapsed";
                var expStyle2 = "style=\"display: none;\"";
                var expStyle3 = "collapsedLink nlink";
                var isExpanded = !c.IsCollapse;
                if (isExpanded)
                {
                    expStyle1 = "expanded";
                    expStyle2 = "style=\"display: block;\"";
                    expStyle3 = "expandedLink nlink";
                }
                html.Append("<li class=\"" + expStyle1);
                if (!c.SubCategories.Any() && n + 1 == categories.Count())
                {
                    html.Append(" last");
                }
                html.Append("\">");
                html.Append("<a ");
                if (c.SubCategories.Any())
                {
                    html.Append("class=\"" + expStyle3 + "\" ");
                }
                if (c.ParameterDic.Any())
                {
                    foreach (var keyValuePair in c.ParameterDic)
                    {
                        html.Append("" + keyValuePair.Key + "=\"" + keyValuePair.Value + "\" ");
                    }
                }
                if (string.IsNullOrEmpty(c.Url))
                {
                    c.Url = "javascript:void(0); ";
                }
                html.Append("href=\"" + c.Url + "\" ");
                if (!string.IsNullOrEmpty(c.Func))
                {
                    html.Append("onclick=\"" + c.Func + "\" ");
                }
                html.Append("categoryId=\"" + c.Id + "\" ");
                html.Append("group=\"" + c.Group + "\" ");
                html.Append("code=\"" + c.Code + "\" ><p>" + c.DisplayName + "</p></a>");

                if (c.SubCategories.Any())
                {
                    html.Append("<div class=\"main\" " + expStyle2 + " >");
                    html.Append(GetCategory(c.SubCategories));
                    html.Append("</div>");
                }
            }

            html.Append("</ul>");
            return html.ToString();
        }
    }
}
