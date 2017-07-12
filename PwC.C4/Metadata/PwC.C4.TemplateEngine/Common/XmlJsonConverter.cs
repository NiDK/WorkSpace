using System;
using System.Xml;
using Newtonsoft.Json;

namespace PwC.C4.TemplateEngine.Common
{
    public class XmlJsonConverter
    {
        public static string Parse(string json, string root = "Form")
        {
            json = "{\"?xml\":{\"@version\":\"1.0\",\"@standalone\":\"no\"},\"" + root + "\":" + json + "}";
            XmlDocument doc;
            try
            {
                doc = JsonConvert.DeserializeXmlNode(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return doc.InnerXml;
        }
    }
}
