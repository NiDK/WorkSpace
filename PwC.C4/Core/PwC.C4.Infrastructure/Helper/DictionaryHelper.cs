using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwC.C4.Infrastructure.Helper
{
    public static class DictionaryHelper
    {
        public static void Set(this Dictionary<string, string> dic, string key,string value)
        {
            if (dic.ContainsKey(key))
            {
                dic[key] = value;
            }
            else
            {
                dic.Add(key, value);
            }
        }

        public static void Set(this Dictionary<string, object> dic, string key, object value)
        {
            if (dic.ContainsKey(key))
            {
                dic[key] = value;
            }
            else
            {
                dic.Add(key, value);
            }
        }

        public static Dictionary<string, object> ToDictionary(this object source)
        {
            return source.ToDictionary<object>(null);
        }

        public static Dictionary<string, object> ToDictionary(this object source,IList<string> properties)
        {
            return source.ToDictionary<object>(properties);
        }

        public static Dictionary<string, object> ToDictionary(this object source, IList<string> properties,bool isTerm)
        {
            return source.ToDictionary<object>(properties,isTerm);
        }

        public static Dictionary<string, T> ToDictionary<T>(this object source, IList<string> properties, bool isTerm=false)
        {
            if (source == null)
                ThrowExceptionWhenSourceArgumentIsNull();

            var dictionary = new Dictionary<string, T>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
            {
                if (properties == null || properties.Contains(property.Name, StringComparer.OrdinalIgnoreCase))
                    AddPropertyToDictionary<T>(property, source, dictionary, isTerm);
            }

            return dictionary;
        }

        private static void AddPropertyToDictionary<T>(PropertyDescriptor property, object source,
            Dictionary<string, T> dictionary, bool isTerm)
        {
            object value = property.GetValue(source);
            if (IsOfType<T>(value))
            {

                if (isTerm && (string) value != null)
                {
                    var s = (string) value;
                    dictionary.Add(property.Name, (T) (object) s.Trim());
                }
                else
                {
                    dictionary.Add(property.Name, (T) value);
                }
            }

        }

        private static bool IsOfType<T>(object value)
        {
            return value is T;
        }

        private static void ThrowExceptionWhenSourceArgumentIsNull()
        {
            throw new ArgumentNullException("source", "Unable to convert object to a dictionary. The source object is null.");
        }
    }
}
