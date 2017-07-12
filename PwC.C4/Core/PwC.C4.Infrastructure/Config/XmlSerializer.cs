using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using PwC.C4.Infrastructure.BaseLogger;

namespace PwC.C4.Infrastructure.Config
{
    public static class XmlSerializer<T>
    {
        static readonly LogWrapper _log = new LogWrapper();
        public static T DeserializeFromFile()
        {
            try
            {
                var fileName = GetConfigSectionName<T>();
                var path = AppDomain.CurrentDomain.BaseDirectory;
                var conPath = string.Format("{0}\\Configs\\{1}.config", path, fileName);
                using (var stream = new FileStream(conPath, FileMode.Open, FileAccess.Read))
                {
                    T t = Deserialize(stream);
                    stream.Close();
                    return t;
                }
            }
            catch (Exception ee)
            {
                _log.Error("Config file Load Error", ee);
                throw;
            }
            
        }

        static string GetConfigSectionName<T>()
        {
            var t = typeof(T);
            var attrs = t.GetCustomAttributes(typeof(XmlRootAttribute), false);
            return attrs.Length > 0 ? ((XmlRootAttribute)attrs[0]).ElementName : t.Name;
        }

        public static T Deserialize(Stream stream)
        {
            var xser = new XmlSerializer(typeof(T));
            try
            {
                return (T)xser.Deserialize(stream);
            }
            catch (Exception ee)
            {
                string ss = ee.ToString();
                throw;
            }
            
        }

        public static T Deserialize(TextReader textReader)
        {
            var xser = new XmlSerializer(typeof(T));

            return (T)xser.Deserialize(textReader);
        }

        public static T Deserialize(XmlReader xmlReader)
        {
            var xser = new XmlSerializer(typeof(T));

            return (T)xser.Deserialize(xmlReader);
        }

        public static T Deserialize(XmlReader xmlReader, string encodingStyle)
        {
            var xser = new XmlSerializer(typeof(T));

            return (T)xser.Deserialize(xmlReader, encodingStyle);
        }


        public static void SerializeToFile(string fileName, T t)
        {
            using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                Serialize(stream, t);
                stream.Close();
            }
        }

        public static void Serialize(Stream stream, T t)
        {
            var xser = new XmlSerializer(typeof(T));
            xser.Serialize(stream, t);
        }

        public static void Serialize(TextWriter writer, T t)
        {
            var xser = new XmlSerializer(typeof(T));
            xser.Serialize(writer, t);
        }


        public static void Serialize(XmlWriter writer, T t)
        {
            var xser = new XmlSerializer(typeof(T));
            xser.Serialize(writer, t);
        }

        public static string ToString(T t)
        {
            using (var writer = new StringWriter())
            {
                Serialize(writer, t);
                return writer.ToString();
            }
        }

        public static T FromString(string str)
        {
            using (var reader = new StringReader(str))
            {
                return Deserialize(reader);
            }
        }
    }
}
