using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PwC.C4.Metadata.Metadata;

namespace PwC.C4.Metadata.Metadata
{
    public class DynamicMetadataConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var model = (DynamicMetadata)value;
            var contractResolver = serializer.ContractResolver as DynamicMetadataContractResolver;
            HashSet<string> serializeProperties = null;
            Dictionary<string, string> shortNames = null;
            bool customSerialize = false;
            if (contractResolver != null)
            {
                customSerialize = true;
                serializeProperties = contractResolver.GetSerializedProperties(value.GetType());
                if (serializeProperties == null)
                {
                    serializeProperties = new HashSet<string>(model.Properties.Keys);
                }
                HashSet<string> ignorProps = contractResolver.GetIgnorProperties(value.GetType());
                if (ignorProps != null)
                {
                    serializeProperties.RemoveWhere(ignorProps.Contains);
                }
                shortNames = contractResolver.GetSerializeNames(value.GetType());

            }
            writer.WriteStartObject();
            foreach (var prop in model.Properties)
            {
                if (serializer.NullValueHandling == NullValueHandling.Ignore && prop.Value == null)
                {
                    continue;
                }
                
                if (customSerialize)
                {
                    if (!serializeProperties.Contains(prop.Key))
                    {
                        continue;
                    }
                }

                if (shortNames != null)
                {
                    string name = null;
                    if (shortNames.TryGetValue(prop.Key, out name))
                    {
                        writer.WritePropertyName(name);
                    }
                    else
                    {
                        writer.WritePropertyName(prop.Key);
                    }
                }
                else
                {
                    writer.WritePropertyName(prop.Key);
                }
                serializer.Serialize(writer, prop.Value);
            }

            writer.WriteEndObject();
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var model = (DynamicMetadata)Activator.CreateInstance(objectType);
            while (reader.Read())
            {
                var key = reader.Value as string;
                if (string.IsNullOrEmpty(key))
                    break;
                reader.Read();
                object value = null;
                if (reader.ValueType == null)
                {
                    value = serializer.Deserialize(reader, typeof(DynamicMetadata));
                }
                else
                {
                    value = serializer.Deserialize(reader, reader.ValueType);
                }
                model.Properties.Add(key, value);
            }

            return model;
        }

        public override bool CanRead
        {
            get { return true; }
        }
        public override bool CanWrite
        {
            get { return true; }
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(DynamicMetadata).IsAssignableFrom(objectType);
        }
    }

   
}
