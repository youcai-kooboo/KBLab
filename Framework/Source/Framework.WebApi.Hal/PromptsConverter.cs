using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Framework.WebApi.Hal
{
    /// <summary>
    /// Converts prompts of the representation to and from JSON. 
    /// </summary>
    public class PromptsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof (Dictionary<string, string>).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Dictionary<string, string> prompts = (Dictionary<string, string>)value;
            writer.WriteStartObject();
            foreach(KeyValuePair<string, string> prompt in prompts)
            {
               // writer.WriteRawValue(string.Format("\"{0}\":\"{1}\"", prompt.Key, prompt.Value));
                writer.WritePropertyName(prompt.Key);
                serializer.Serialize(writer, prompt.Value);
                //writer.WriteValue(prompt.Value);
            }
            writer.WriteEndObject();
        }
    }
}