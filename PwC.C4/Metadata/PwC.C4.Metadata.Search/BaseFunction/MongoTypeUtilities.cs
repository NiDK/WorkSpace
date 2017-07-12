using System;
using System.Collections.Generic;
using System.Globalization;
using MongoDB.Bson;
using PwC.C4.Metadata.Metadata;
using PwC.C4.Metadata.Service;

namespace PwC.C4.Metadata.Search.BaseFunction
{
    public static class MongoTypeUtilities
    {
        public static BsonValue BsonValueConverter(string type, string value)
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
                case "int64":
                    var intl = 0L;
                    long.TryParse(value, out intl);
                    return intl;
                case "bool":
                case "boolean":
                case "bit":
                    var bintv = 0;
                    var resl = int.TryParse(value, out bintv);
                    if (resl)
                    {
                        return bintv;
                    }
                    var boolv = false;
                    bool.TryParse(value, out boolv);
                    return boolv;
                case "date":
                case "datetime":
                    DateTime datev;
                    System.IFormatProvider format = new System.Globalization.CultureInfo("zh-HK", true);
                    if (DateTime.TryParse(value, format, DateTimeStyles.AdjustToUniversal, out datev))
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
                case "double":           
                case "decimal":
                case "money":
                    var doublev = 0d;
                    double.TryParse(value, out doublev);
                    return doublev;
                default:
                    return value;
            }

        }

        public static Dictionary<string, object> DatetimeFix(Dictionary<string, object> prop, string entityName)
        {
            var col = ColumnService.Instance().GetAllDatetimeColumn<DynamicMetadata>(entityName);
            var newDic = new Dictionary<string, object>();
            foreach (var keyValuePair in prop)
            {
               
                if (col.Contains(keyValuePair.Key) && keyValuePair.Value != null)
                {
                    var dt = Convert.ToDateTime(keyValuePair.Value);
                    newDic.Add(keyValuePair.Key, dt.ToLocalTime());
                }
                else
                {
                    newDic.Add(keyValuePair.Key, keyValuePair.Value);
                }
                
            }
            return newDic;
        }
    }
}
