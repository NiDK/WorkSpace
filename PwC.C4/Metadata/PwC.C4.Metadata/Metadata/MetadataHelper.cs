using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Metadata.Attributes;
using PwC.C4.Metadata.Config;

namespace PwC.C4.Metadata.Metadata
{
    public static class MetadataHelper
    {
        public static Dictionary<string, object> ToObjects<T>(Dictionary<string, string> dic,string entityName=null)
        {
            var entity = GetEntityName<T>(entityName);
            var result = new Dictionary<string, object>();
            foreach (var d in dic)
            {
                var col = MetadataSettings.Instance.GetColumn(entity, d.Key);
                var conValue = TypeHelper.TypeConverter(col.Type, d.Value);

                result.Add(d.Key, conValue);
            }
            return result;
        }

        public static Dictionary<string, object> ToObjects(string json, string entityName)
        {
            var mintset = MetadataSettings.Instance;
            var result = new Dictionary<string, object>();
            var dic = JsonHelper.Deserialize<Dictionary<string, string>>(json);
            var mappingCol = mintset.GetMappingColumns(entityName);
            var arrayCol = mintset.GetArrayColumns(entityName);
            foreach (var d in dic)
            {
                if (mappingCol.ContainsKey(d.Key))
                {
                    var conValue = ToObjects(d.Value, mappingCol[d.Key]);
                    result.Add(d.Key, conValue);
                }
                else if (arrayCol.ContainsKey(d.Key))
                {
                    if (TypeHelper.IsConvertableType(arrayCol[d.Key]))
                    {
                        var type = TypeHelper.GetType(arrayCol[d.Key]);
                        var genericListType = typeof(List<>);
                        var specificListType = genericListType.MakeGenericType(type);
                        var list = Activator.CreateInstance(specificListType);
                    }
                }
                else
                {
                    var col = mintset.GetColumn(entityName, d.Key);
                    var conValue = TypeHelper.TypeConverter(col.Type, d.Value);
                    result.Add(d.Key, conValue);
                }
            }
            return result;
        } 

        public static string GetEntityName<T>(string entityName=null)
        {
            if (entityName != null)
            {
                return entityName;
            }
            var classAttribute =
                    (MetaObjectAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(MetaObjectAttribute));
            var tableName = classAttribute.Name;
            return tableName;
        }

        public static string GetPrimaryKey<T>(string entityName = null)
        {
            string pkName = null;
            if (entityName != null)
            {
                pkName = MetadataSettings.Instance.GetPrimaryKey(entityName);
                if (pkName != null)
                    return pkName;
            }
            var props = typeof(T).GetProperties().ToList();
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

        public static void ClearSettingCache()
        {
            MetadataSettingsExtension.ReloadConfig();
        }
    }
}
