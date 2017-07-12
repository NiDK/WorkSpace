using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using PwC.C4.TemplateEngine.Extensions;
using RazorEngine.Templating;
using RazorEngine.Text;

namespace PwC.C4.TemplateEngine
{
    [RequireNamespaces("System.Web.Mvc.Html")]
    public class HtmlTemplateBase<T>:TemplateBase<T>, IViewDataContainer
    {
        #region Base function

        private HtmlHelper<T> helper = null;
        private ViewDataDictionary viewdata = null;
        private System.Dynamic.DynamicObject viewbag = null;
        private UrlHelper urlhelper = null;
        private RenderHelper renderHelper = null;
        private RushRenderHelper rushRenderHelper = null;
        private MetadataRender metadataRenderHelper = null;
        
        public dynamic ViewBag
        {
            get
            {
                return (WebPageContext.Current.Page as WebViewPage).ViewBag;
            }
        }

        public HtmlHelper<T> Html
        {
            get
            {
                if (helper == null)
                {
                    var p = WebPageContext.Current;
                    var wvp = p.Page as WebViewPage;
                    var context = wvp != null ? wvp.ViewContext : null;

                    helper = new HtmlHelper<T>(context, this);
                }
                return helper;
            }
        }

        public RenderHelper Render
        {
            get { return renderHelper ?? (renderHelper = new RenderHelper()); }
        }


        public RushRenderHelper RushRender
        {
            get { return rushRenderHelper ?? (rushRenderHelper = new RushRenderHelper()); }
        }

        public MetadataRender MetadataHelper
        {
            get { return metadataRenderHelper ?? (metadataRenderHelper = new MetadataRender()); }
        }


        public UrlHelper Url
        {
            get
            {
                if (urlhelper == null)
                {
                    urlhelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
                }
                return urlhelper;
            }
        }

        public ViewDataDictionary ViewData
        {
            get
            {
                if (viewbag == null)
                {
                    var p = WebPageContext.Current;
                    var viewcontainer = p.Page as IViewDataContainer;
                    viewdata = new ViewDataDictionary(viewcontainer.ViewData);

                    if (this.Model != null)
                    {
                        viewdata.Model = Model;
                    }

                }

                return viewdata;
            }
            set
            {
                viewdata = value;
            }
        }

        public override void WriteTo(TextWriter writer, object value)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            if (value == null) return;

            var encodedString = value as IEncodedString;
            if (encodedString != null)
            {
                writer.Write(encodedString);
            }
            else
            {
                var htmlString = value as IHtmlString;
                if (htmlString != null)
                    writer.Write(htmlString.ToHtmlString());
                else
                {
                    //This was the base template's implementation:
                    encodedString = TemplateService.EncodedStringFactory.CreateEncodedString(value);
                    writer.Write(encodedString);
                }
            }
        }
        #endregion


    }

}
