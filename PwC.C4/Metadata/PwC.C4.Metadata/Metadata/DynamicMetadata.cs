using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using PwC.C4.Metadata.Attributes;
using PwC.C4.Metadata.Config;

namespace PwC.C4.Metadata.Metadata
{
    [JsonConverter(typeof(DynamicMetadataConverter))]
    [Serializable]
    public class DynamicMetadata : DynamicObject, ICloneable
    {
        public DynamicMetadata()
        {
             Properties = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }
        public static class Constans
        {
            public const int EmptyInt = -32767;
            public const string EmptyIntString = "-32767";
            public static readonly DateTime EmptyDate = new DateTime(1900, 1, 1);
            public const string EmptyDateString = "1900.01.01";
            public static readonly DateTime MinEmptyDate = DateTime.MinValue;
            public const string MinDateString = "0001.01.01";
        }

        public string CreateBy
        {
            get { return SafeGet<string>("CreateBy"); }
            set { SafeSet("CreateBy", value); }
        }

        public string ModifyBy
        {
            get { return SafeGet<string>("ModifyBy"); }
            set { SafeSet("ModifyBy", value); }
        }

        public DateTime CreateDate
        {
            get { return SafeGet<DateTime>("CreateDate"); }
            set { SafeSet("CreateDate", value); }
        }

        public DateTime ModifyDate
        {
            get { return SafeGet<DateTime>("ModifyDate"); }
            set { SafeSet("ModifyDate", value); }
        }

        [CustomMetaColumn]
        public Dictionary<string, object> Properties { get; set; }

        private static T DefaultValue<T>()
        {
            object value = null;
            if (typeof(int) == typeof(T))
            {
                value = 0;
            }
            else if (typeof(string) == typeof(T))
            {
                value = string.Empty;
            }
            else
            {
                value = default(T);
            }

            return (T)value;
        }

        public bool IsTranslatored { get; set; }
        public object this[string name]
        {
            get
            {
                if (Properties.ContainsKey(name))
                {
                    return Properties[name];
                }
                return null;
            }
            set { Properties[name] = value; }
        }
        public bool HasProperty(string propertyName)
        {
            return Properties.ContainsKey(propertyName);
        }

        public string SafeGetYesNo(string value)
        {
            var val = SafeGet<bool>(value);
            return val ? "Yes" : "No";
        }

        public string SafeGetEnumDesc<T>(string value)
        {
            var item = SafeGet<T>(value);
            if (value == null)
            {
                return string.Empty;
            }
            try
            {
                var enumType = item.GetType();
                DescriptionAttribute dna = null;

                FieldInfo fi = enumType.GetField(System.Enum.GetName(enumType, item));
                dna = (DescriptionAttribute)Attribute.GetCustomAttribute(
                    fi, typeof(DescriptionAttribute));

                if (dna != null && string.IsNullOrEmpty(dna.Description) == false)
                    return dna.Description;
            }
            catch
            {
            }
            return item.ToString();
        }

        public object SafeGet(string propertyName, object defaultValue)
        {
            if (HasProperty(propertyName))
                return Properties[propertyName];
            else
            {
                return defaultValue;
            }

        }
        public T SafeGet<T>(string propertyName, T defaultValue)
        {
            if (HasProperty(propertyName))
            {
                try
                {
                    if (typeof(T) != typeof(int)) return (T)Properties[propertyName];
                    var val = Convert.ToInt32(Properties[propertyName]);
                    return (T)(object)val;
                }
                catch (Exception ex)
                {
                    //return default(T);
                    return defaultValue;
                }

            }
            else
            {
                return defaultValue;
            }
        }
        public T SafeGet<T>(string propertyName)
        {
            return SafeGet(propertyName, DefaultValue<T>());
        }

        public void SafeSet<T>(string propertyName, T value)
        {
            if (value == null)
                value = DefaultValue<T>();

            if (HasProperty(propertyName))
                Properties[propertyName] = value;
            else
            {
                Properties.Add(propertyName, value);
            }
        }

        public virtual Guid UniqueId { get; set; }

        public void SetProperty(string name, object value)
        {
            if(Properties.ContainsKey(name))
                Properties[name] = value;
            else
            {
                Properties.Add(name,value);
            }
        }

        public void SetProperties(Dictionary<string, object> prop)
        {
            Properties = prop;
        }

        public object GetProperty(string name)
        {
            object obj;
            Properties.TryGetValue(name, out obj);
            return obj;
        }

        public void RemoveProperty(string name)
        {
            if (Properties.ContainsKey(name))
                Properties.Remove(name);
        }
        public void RemoveOriginalProperty()
        {
            var keys = Properties.Keys.Where(key => key.Contains("Original-")).ToList();
            keys.ForEach(RemoveProperty);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var name = binder.Name.ToLower();
            return Properties.TryGetValue(name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            Properties[binder.Name.ToLower()] = value;
            return true;
        }


        public object Clone()
        {
            var newModel = (DynamicMetadata)Activator.CreateInstance(this.GetType());
            foreach (var property in Properties)
            {
                newModel.Properties.Add(property.Key, property.Value);
            }
            return newModel;
        }

        public string GetEntityName(string entityName=null)
        {
            if (entityName != null)
                return entityName;
            var classAttribute =
                    (MetaObjectAttribute)Attribute.GetCustomAttribute(this.GetType(), typeof(MetaObjectAttribute));
            var tableName = classAttribute.Name;
            return tableName;
        }

        public string GetPrimaryKey(string entityName = null)
        {
            if (entityName != null)
            {
                return MetadataSettings.Instance.GetPrimaryKey(entityName);
            }
            var pkName = "";
            var props = this.GetType().GetProperties().ToList();
            props.ForEach(p =>
            {
                var keys = p.GetCustomAttributes(typeof(MetaColumnAttribute), true);
                if (keys.Length != 1) return;
                var attr = (MetaColumnAttribute)keys[0];
                if (attr.IsPk)
                {
                    pkName = attr.Name;
                }
            });
            return pkName;
        }

        public string GetPrimaryValue(string entityName = null)
        {
            var data = this.GetType().GetProperty(GetPrimaryKey(entityName)).GetValue(this, null);
            var id = data?.ToString() ?? string.Empty;
            return id;
        }
    }

}
