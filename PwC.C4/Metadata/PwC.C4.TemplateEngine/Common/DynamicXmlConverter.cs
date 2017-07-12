using System.Dynamic;
using System.Linq;
using System.Xml.Linq;

namespace PwC.C4.TemplateEngine.Common
{

    public  class DynamicXmlConverter : DynamicObject
    {
        XElement _root;

        private DynamicXmlConverter(XElement root)
        {
            _root = root;
        }

        public static DynamicXmlConverter Parse(XElement root)
        {
            return new DynamicXmlConverter(root);
        }

        public static DynamicXmlConverter Parse(string xmlString)
        {
            return new DynamicXmlConverter(XDocument.Parse(xmlString).Root);
        }

        public static DynamicXmlConverter Load(string filename)
        {
            return new DynamicXmlConverter(XDocument.Load(filename).Root);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = string.Empty;

            var att = _root.Attribute(binder.Name);
            if (att != null)
            {
                result = att.Value;
                return true;
            }

            var nodes = _root.Elements(binder.Name);
            if (nodes.Count() > 1)
            {
                result = nodes.Select(n => new DynamicXmlConverter(n)).ToList();
                return true;
            }

            var node = _root.Element(binder.Name);
            if (node != null)
            {
                if (node.HasElements)
                {
                    result = new DynamicXmlConverter(node);
                }
                else
                {
                    result = node.Value;
                }
                return true;
            }

            return true;
        }
    }
   


}