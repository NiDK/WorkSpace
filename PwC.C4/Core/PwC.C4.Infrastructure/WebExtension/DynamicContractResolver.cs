using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PwC.C4.Infrastructure.WebExtension
{
    ///<summary>
    ///</summary>
    public class DynamicContractResolver : DefaultContractResolver
    {
        private readonly Dictionary<Type, IEnumerable<string>> _customProperties = new Dictionary<Type, IEnumerable<string>>();
        private readonly bool _serialize;

        ///<summary>
        ///</summary>
        ///<param name="customProperties">序列化的属性或对象</param>
        ///<param name="serialize">是否需要序列化该属性或对象</param>
        public DynamicContractResolver(Dictionary<Type, IEnumerable<string>> customProperties, bool serialize)
        {
            _customProperties = customProperties;
            _serialize = serialize;
            
        }
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {

            IList<JsonProperty> properties = base.CreateProperties(type, MemberSerialization.Fields);
            IEnumerable<string> customProperties = null;
            
            if (_serialize)
            {
                if (_customProperties.TryGetValue(type, out customProperties))
                {
                    var pr = from prop in properties
                             where customProperties.Contains(prop.PropertyName, StringComparer.OrdinalIgnoreCase)
                             select prop;
                    properties = pr.ToList();
                }
            }
            else
            {
                if (_customProperties.TryGetValue(type, out customProperties))
                {
                    var pr = from prop in properties
                             where !customProperties.Contains(prop.PropertyName, StringComparer.OrdinalIgnoreCase)
                             select prop;
                    properties = pr.ToList();
                }
                
            }
               
            return properties;
        }
        

    }
    
}
