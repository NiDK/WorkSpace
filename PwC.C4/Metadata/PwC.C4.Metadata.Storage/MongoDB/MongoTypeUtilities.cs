using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using PwC.C4.Infrastructure.Helper;
using PwC.C4.Metadata.Config;
using PwC.C4.Metadata.Metadata;
using PwC.C4.Metadata.Service;

namespace PwC.C4.Metadata.Storage.MongoDB
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
                    DateTime dt;
                    DateTime.TryParse(keyValuePair.Value.ToString(), out dt);
                    newDic.Add(keyValuePair.Key, dt.ToLocalTime());
                }
                else
                {
                    newDic.Add(keyValuePair.Key, keyValuePair.Value);
                }
                
            }
            return newDic;
        }

        public static DynamicMetadata ToMetadata(this BsonDocument bson)
        {
            var meta = new DynamicMetadata {Properties = new Dictionary<string, object>()};
            if (bson?.Elements != null && bson.ElementCount > 0)
            {
                foreach (var bsonElement in bson.Elements)
                {
                    if (bsonElement.Value.IsBsonArray)
                    {
                        var array = bsonElement.Value.AsBsonArray;
                        if (array != null && array.Count > 0)
                        {
                            var firstOrDefault = array.FirstOrDefault();
                            if (firstOrDefault != null && firstOrDefault.IsBsonDocument)
                            {
                                var dmList = array.Select(a => a.AsBsonDocument.ToMetadata()).ToList();
                                meta.Properties.Add(bsonElement.Name, dmList);
                            }
                            else
                            {
                                meta.Properties.Add(bsonElement.Name, BsonTypeMapper.MapToDotNetValue(array));
                            }
                        }
                        else
                        {
                            meta.Properties.Add(bsonElement.Name, null);
                        }
                    }
                    else if (bsonElement.Value.IsValidDateTime)
                    {
                        meta.Properties.Add(bsonElement.Name, bsonElement.Value.ToNullableUniversalTime());
                    }
                    else if (bsonElement.Value.IsBoolean)
                    {
                        meta.Properties.Add(bsonElement.Name, bsonElement.Value.AsNullableBoolean);
                    }
                    else if (bsonElement.Value.IsBsonUndefined || bsonElement.Value.IsBsonNull)
                    {
                        meta.Properties.Add(bsonElement.Name, null);
                    }
                    else if (bsonElement.Value.IsGuid)
                    {
                        meta.Properties.Add(bsonElement.Name, bsonElement.Value.AsNullableGuid);
                    }
                    else if (bsonElement.Value.IsDouble)
                    {
                        meta.Properties.Add(bsonElement.Name, bsonElement.Value.AsNullableDouble);
                    }
                    else if (bsonElement.Value.IsInt32)
                    {
                        meta.Properties.Add(bsonElement.Name, bsonElement.Value.AsNullableInt32);
                    }
                    else if (bsonElement.Value.IsInt64)
                    {
                        meta.Properties.Add(bsonElement.Name, bsonElement.Value.AsNullableInt64);
                    }
                    else if (bsonElement.Value.IsBsonBinaryData)
                    {
                        meta.Properties.Add(bsonElement.Name, bsonElement.Value.AsByteArray);
                    }
                    else if (bsonElement.Value.IsBsonDocument)
                    {
                        meta.Properties.Add(bsonElement.Name, bsonElement.Value.AsBsonDocument.ToMetadata());
                    }
                    else
                    {
                        meta.Properties.Add(bsonElement.Name, bsonElement.Value.AsString);
                    }
                }
            }
            
            return meta;
        }

        public static Dictionary<string, object> ToDic(this BsonDocument bson)
        {
            var dic = new Dictionary<string, object>();
            if (bson?.Elements != null && bson.ElementCount > 0)
            {
                foreach (var bsonElement in bson.Elements)
                {
                    if (bsonElement.Value.IsBsonArray)
                    {
                        var array = bsonElement.Value.AsBsonArray;
                        if (array != null && array.Count > 0)
                        {
                            var firstOrDefault = array.FirstOrDefault();
                            if (firstOrDefault != null && firstOrDefault.IsBsonDocument)
                            {
                                var dmList = array.Select(a => a.AsBsonDocument.ToDic()).ToList();
                                dic.Add(bsonElement.Name, dmList);
                            }
                            else
                            {
                                dic.Add(bsonElement.Name, BsonTypeMapper.MapToDotNetValue(array));
                            }
                        }
                        else
                        {
                            dic.Add(bsonElement.Name, null);
                        }
                    }
                    else if (bsonElement.Value.IsValidDateTime)
                    {
                        dic.Add(bsonElement.Name, bsonElement.Value.ToNullableUniversalTime());
                    }
                    else if (bsonElement.Value.IsBoolean)
                    {
                        dic.Add(bsonElement.Name, bsonElement.Value.AsNullableBoolean);
                    }
                    else if (bsonElement.Value.IsBsonUndefined || bsonElement.Value.IsBsonNull)
                    {
                        dic.Add(bsonElement.Name, null);
                    }
                    else if (bsonElement.Value.IsGuid)
                    {
                        dic.Add(bsonElement.Name, bsonElement.Value.AsNullableGuid);
                    }
                    else if (bsonElement.Value.IsDouble)
                    {
                        dic.Add(bsonElement.Name, bsonElement.Value.AsNullableDouble);
                    }
                    else if (bsonElement.Value.IsInt32)
                    {
                        dic.Add(bsonElement.Name, bsonElement.Value.AsNullableInt32);
                    }
                    else if (bsonElement.Value.IsInt64)
                    {
                        dic.Add(bsonElement.Name, bsonElement.Value.AsNullableInt64);
                    }
                    else if (bsonElement.Value.IsBsonBinaryData)
                    {
                        dic.Add(bsonElement.Name, bsonElement.Value.AsByteArray);
                    }
                    else if (bsonElement.Value.IsBsonDocument)
                    {
                        dic.Add(bsonElement.Name, bsonElement.Value.AsBsonDocument.ToDic());
                    }
                    else
                    {
                        dic.Add(bsonElement.Name, bsonElement.Value.AsString);
                    }
                }
            }
            return dic;
        }
    }
}
