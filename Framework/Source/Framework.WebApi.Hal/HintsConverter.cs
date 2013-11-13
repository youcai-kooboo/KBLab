using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Framework.WebApi.Hal
{
    public class HintsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof (Dictionary<string,object>).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        JsonSerializer serializer)
        {
            return reader.Value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Dictionary<string, object> hints = (Dictionary<string, object>)value;

            foreach (KeyValuePair<string, object> hint in hints)
            {
                writer.WritePropertyName(hint.Key);
                writer.WriteValue(hint.Value);
            }
        }
    }
}