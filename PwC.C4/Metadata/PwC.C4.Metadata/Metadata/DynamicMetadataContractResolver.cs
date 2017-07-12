using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PwC.C4.Metadata.Metadata
{
    public class DynamicMetadataContractResolver : DefaultContractResolver
    {
        internal Dictionary<Type,HashSet<string>> SerializeProperties;
        internal Dictionary<Type,HashSet<string>> IgnorProperties;
        internal Dictionary<Type, Dictionary<string, string>> PropertyNameMapping;
        public DynamicMetadataContractResolver(
            Dictionary<Type,IEnumerable<string>> serializeProps,
            Dictionary<Type,Dictionary<string,string>>  shortNames,
            Dictionary<Type,IEnumerable<string>> ignorProps)
        {
            if (serializeProps != null)
            {
                SerializeProperties = new Dictionary<Type, HashSet<string>>(serializeProps.Count);
                foreach (KeyValuePair<Type, IEnumerable<string>> props in serializeProps)
                {
                    SerializeProperties.Add(props.Key,new HashSet<string>(props.Value,StringComparer.OrdinalIgnoreCase));
                }
                //new HashSet<string>(serializeProps, StringComparer.OrdinalIgnoreCase);
            }
            PropertyNameMapping = shortNames;
            if (ignorProps != null)
            {
                IgnorProperties = new Dictionary<Type, HashSet<string>>(ignorProps.Count);// new HashSet<string>(ignorProps);
                foreach (var kv in ignorProps)
                {
                    ignorProps.Add(kv.Key,new HashSet<string>(kv.Value,StringComparer.OrdinalIgnoreCase));
                }
            }
        }

        public DynamicMetadataContractResolver(Dictionary<Type, Dictionary<string, string>> serializeProps, Dictionary<Type, Dictionary<string, string>> shortNames)
            :this(null,shortNames,null)
        {
            
        }
        public DynamicMetadataContractResolver(Dictionary<Type, IEnumerable<string>> serializeProps,
                                            Dictionary<Type, Dictionary<string, string>> shortNames)
            :this(serializeProps,shortNames,null)
        {
            
        }
        public HashSet<string> GetSerializedProperties(Type type)
        {
            if (SerializeProperties == null)
                return null;
            HashSet<string> retval = null;
            SerializeProperties.TryGetValue(type, out retval);
            return retval;
        }
        public HashSet<string> GetIgnorProperties(Type type)
        {
            if (IgnorProperties == null)
                return null;
            HashSet<string> retval = null;
            IgnorProperties.TryGetValue(type, out retval);
            return retval;
        }
        public Dictionary<string, string> GetSerializeNames(Type type)
        {
            if (PropertyNameMapping == null)
                return null;
            Dictionary<string, string> names = null;
            PropertyNameMapping.TryGetValue(type, out names);
            return names;
        }

        protected override JsonDictionaryContract CreateDictionaryContract(Type objectType)
        {
            JsonDictionaryContract contract = base.CreateDictionaryContract(objectType);
            return contract;
        }
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properteis = base.CreateProperties(type, memberSerialization);
            var serializeProperties = GetSerializedProperties(type);
            if (serializeProperties == null)
            {
                serializeProperties = new HashSet<string>(properteis.Select(p=>p.PropertyName),StringComparer.OrdinalIgnoreCase);
            }
            HashSet<string> ignorProps = GetIgnorProperties(GetType());
            if (ignorProps != null)
            {
                serializeProperties.RemoveWhere(ignorProps.Contains);
            }
            var shortNames = GetSerializeNames(type);
            var props= properteis.Where(p => serializeProperties.Contains(p.PropertyName)).ToList();
            if (shortNames != null)
            {
                foreach (var jsonProperty in props)
                {
                    string pname = null;
                    if(shortNames.TryGetValue(jsonProperty.PropertyName,out pname))
                    {
                        jsonProperty.PropertyName = pname;
                    }
                }
            }
            return props;
        }

        //protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        //{

        //    IList<JsonProperty> properties = base.CreateProperties(type, MemberSerialization.Fields);
        //    IEnumerable<string> customProperties = null;

        //    if (_serialize)
        //    {
        //        if (_customProperties.TryGetValue(type, out customProperties))
        //        {
        //            var pr = from prop in properties
        //                     where customProperties.Contains(prop.PropertyName, StringComparer.OrdinalIgnoreCase)
        //                     select prop;
        //            properties = pr.ToList();
        //        }
        //    }
        //    else
        //    {
        //        if (_customProperties.TryGetValue(type, out customProperties))
        //        {
        //            var pr = from prop in properties
        //                     where !customProperties.Contains(prop.PropertyName, StringComparer.OrdinalIgnoreCase)
        //                     select prop;
        //            properties = pr.ToList();
        //        }

        //    }

        //    return properties;
        //}
    }
}
