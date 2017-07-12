using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace PwC.C4.Configuration.Messager.Service
{
    public static class CommonService
    {
        public static string Beautify(this XmlDocument doc)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };
            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                doc.Save(writer);
            }
            return sb.ToString();
        }

        public static string CheckXml(string xml)
        {
            XmlTextReader r = new XmlTextReader(new StringReader(xml));
            try
            {
                while (r.Read())
                {
                }
                return "";

            }
            catch (Exception ee)
            {
                return ee.Message;
            }
            finally
            {
                r.Close();
            }
        }

    }
}
