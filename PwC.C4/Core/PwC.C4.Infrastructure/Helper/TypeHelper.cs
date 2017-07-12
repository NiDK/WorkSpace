using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PwC.C4.Infrastructure.Helper
{
    public static class TypeHelper
    {
        private static readonly Dictionary<string, Type> TypeDic;

        private static readonly List<string> ConvertableType = new List<string>()
        {
            "int","int16","int32","bool","boolean","bit","date","datetime","guid","float","double","decimal","money","string"
        };

        static TypeHelper()
        {
            if(TypeDic==null)
                TypeDic= new Dictionary<string, Type>();
        }

        public static T StringToObject<T>(string value)
        {
            try
            {
                return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// Given an object of anonymous type, add each property as a key and associated with its value to a dictionary.
        ///
        /// This helper will cache accessors and types, and is intended when the anonymous object is accessed multiple
        /// times throughout the lifetime of the web application.
        /// </summary>
        public static Dictionary<string,object> ObjectToDictionary(object value)
        {
            var dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            if (value != null)
            {
                foreach (PropertyHelper helper in PropertyHelper.GetProperties(value))
                {
                    dictionary.Add(helper.Name, helper.GetValue(value));
                }
            }

            return dictionary;
        }

        /// <summary>
        /// Given an object of anonymous type, add each property as a key and associated with its value to a dictionary.
        ///
        /// This helper will not cache accessors and types, and is intended when the anonymous object is accessed once
        /// or very few times throughout the lifetime of the web application.
        /// </summary>
        public static Dictionary<string, object> ObjectToDictionaryUncached(object value)
        {
            var dictionary = new Dictionary<string, object>();

            if (value != null)
            {
                foreach (PropertyHelper helper in PropertyHelper.GetProperties(value))
                {
                    dictionary.Add(helper.Name, helper.GetValue(value));
                }
            }

            return dictionary;
        }

        public static object TypeConverter(string type, string value)
        {
            type = type.ToLower();
            //可空类型处理
            if (type.Contains("?"))
            {
                if (value == null)
                {
                    return null;
                }
                else
                {
                    type = type.Replace("?", "");
                }
            }
            switch (type)
            {
                case "int":
                case "int16":
                case "int32":
                    var intv = 0;
                    int.TryParse(value, out intv);
                    return intv;
                case "bool":
                case "boolean":
                case "bit":
                    var boolv = false;
                    bool.TryParse(value, out boolv);
                    return boolv;
                case "date":
                case "datetime":
                    DateTime datev;
                    System.IFormatProvider format = new System.Globalization.CultureInfo("zh-HK", true);
                    if (DateTime.TryParse(value, format, DateTimeStyles.AdjustToUniversal,out datev))
                    {
                        return datev;
                    }
                    else
                    {
                        return null;
                    }
                case "guid":
                    Guid guidv;
                    Guid.TryParse(value, out guidv);
                    return guidv;
                case "float":
                    var floatv = 0f;
                    float.TryParse(value, out floatv);
                    return floatv;
                case "double":
                    var doublev = 0d;
                    double.TryParse(value, out doublev);
                    return doublev;
                case "decimal":
                case "money":
                    var dc = new decimal();
                    decimal.TryParse(value, out dc);
                    return dc;
                default:
                    return value;
            }
        }

        public static bool IsConvertableType(string type)
        {
            type = type.ToLower();
            return ConvertableType.Contains(type);
        }

        public static Type GetType(string type)
        {
            type = type.ToLower();
            if (TypeDic.ContainsKey(type))
                return TypeDic[type];
            switch (type)
            {
                case "string":
                    var st = typeof (string);
                    if(!TypeDic.ContainsKey(type))
                        TypeDic.Add(type, st);
                    return st;
                case "int":
                case "int16":
                case "int32":
                    var it = typeof (int);
                    if(!TypeDic.ContainsKey(type))
                        TypeDic.Add(type, it);
                    return it;
                case "bool":
                case "boolean":
                case "bit":
                    var bt = typeof (bool);
                    if(!TypeDic.ContainsKey(type))
                        TypeDic.Add(type, bt);
                    return bt;
                case "date":
                case "datetime":
                    var tt = typeof (DateTime);
                    if(!TypeDic.ContainsKey(type))
                        TypeDic.Add(type, tt);
                    return tt;
                case "guid":
                    var gt = typeof(Guid);
                    if (!TypeDic.ContainsKey(type))
                        TypeDic.Add(type, gt);
                    return gt;
                case "float":
                    var ft = typeof(float);
                    if (!TypeDic.ContainsKey(type))
                        TypeDic.Add(type, ft);
                    return ft;
                case "double":
                    var dt = typeof(double);
                    if (!TypeDic.ContainsKey(type))
                        TypeDic.Add(type, dt);
                    return dt;
                case "decimal":
                case "money":
                    var dmt = typeof(decimal);
                    if (!TypeDic.ContainsKey(type))
                        TypeDic.Add(type, dmt);
                    return dmt;
                case "xml":
                    var xmlt = typeof(XmlDocument);
                    if (!TypeDic.ContainsKey(type))
                        TypeDic.Add(type, xmlt);
                    return xmlt;
                default:
                    return typeof (Exception);
            }
        }
    }
}
