using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;
using PwC.C4.Metadata.Metadata;
using PwC.C4.TemplateEngine.Extensions;
using PwC.C4.TemplateEngine.Model.Emnu;

namespace PwC.C4.TemplateEngine.Model
{
    [DataContract]
    public class MetadataEntityControl
    {
        [DataMember]
        public string EntityName { get; set; }

         [DataMember]
        public PageMode Mode { get; set; }

        [DataMember]
        public string DataId { get; set; }

        public DynamicMetadata Metadata { get; set; }

        [DataMember]
        public string EnttiyDesc { get; set; }

        [DataMember]
        public List<MetadataControl> Controls { get; set; }

        public MetadataControl GetControl(string columnName)
        {
            var col = Controls.FirstOrDefault(c => c.Name == columnName);
            if (col == null)
            {
                return new MetadataControl();
            }
            return col;
        }

        public MvcHtmlString GenerateHtml(string columnName, string datasourceGroup, string className,
            object attrObjects, PageMode mode = PageMode.Inherit,bool isHtmlEncoding = true)
        {
            var currentMode = this.Mode;
            if (mode != PageMode.Inherit)
            {
                currentMode = mode;
            }
            switch (currentMode)
            {
                case PageMode.Preview:
                    if (string.IsNullOrEmpty(this.DataId) || this.Metadata == null) return new MvcHtmlString("");
                    object html;
                    if (this.Metadata.IsTranslatored)
                    {
                        html = this.Metadata.GetProperty(columnName);
                    }
                    else
                    {
                        var threadTempDic = new Dictionary<string, Dictionary<object, object>>();
                        var list = new List<DynamicMetadata>() { Metadata };
                        DynamicMetadataTranslator.Translate(list, new List<string>(),
                            m => new DynamicMetadata[] { m }, threadTempDic, new List<string>(), null, EntityName, true);
                        this.Metadata = list.FirstOrDefault();
                        if (this.Metadata == null)
                        {
                            return new MvcHtmlString("Preview Translator Error!");
                        }
                        this.Metadata.IsTranslatored = true;
                        html = this.Metadata.GetProperty(columnName);
                    }
                    if(isHtmlEncoding && html!=null)
                        html = HttpUtility.HtmlEncode(html);
                    return html != null ? new MvcHtmlString(html.ToString()) : new MvcHtmlString("");
                case PageMode.Add: 
                    return MetadataEntityControlExtend.GenerateBy(this.EntityName, columnName, datasourceGroup, className,
                        attrObjects);
                case PageMode.Edit:
                    if (string.IsNullOrEmpty(this.DataId) || this.Metadata == null)
                    {
                        this.Mode = PageMode.Add;
                        return this.GenerateHtml(columnName, datasourceGroup, className, attrObjects);
                    }
                    else
                    {
                        var value = this.Metadata.GetProperty(columnName);
                        var strVal = value != null ? value.ToString() : null;
                        return MetadataEntityControlExtend.GenerateBy(this.EntityName, columnName, datasourceGroup,
                            className,
                            attrObjects, strVal);
                    }
                default:
                    return new MvcHtmlString("Generate Error!");
            }
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            var p = obj as MetadataEntityControl;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return this.EntityName == p.EntityName;
        }

        public override int GetHashCode()
        {
            return this.EntityName.GetHashCode();
        }
    }

    public static class MetadataEntityControlExtend
    {
        public static MvcHtmlString GenerateBy(string entityName ,string columnName, string datasourceGroup, string className,
            object attrObjects,string value=null)
        {
            var render = new MetadataRender();
            var obj = render.BuildControl(entityName, columnName, datasourceGroup, className, attrObjects, value);
            if (obj != null && obj.ControlHtml != null)
            {
                return obj.ControlHtml;
            }
            else
            {
                return new MvcHtmlString("Column invalid");
            }
        }
    }
    
}