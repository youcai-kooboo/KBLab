using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Framework.WebApi.Hal
{
    public class LinksConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof (List<Link>).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        JsonSerializer serializer)
        {
            return reader.Value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            List<Link> links = (List<Link>)value;

            writer.WriteStartObject();
            foreach (Link link in links)
            {
                writer.WritePropertyName(link.Rel);

                writer.WriteStartObject();
                writer.WritePropertyName("href");
                writer.WriteValue(link.Href);
                writer.WritePropertyName("method");
                writer.WriteValue(link.Method.ToString());
                if (link.Templated)
                {
                    writer.WritePropertyName("templated");
                    writer.WriteValue(true);
                }
                if (link.Data != null)
                {
                    writer.WritePropertyName("data");
                    writer.WriteStartArray();
                    foreach (LinkQueryParameter dataItem in link.Data)
                    {
                        writer.WriteStartObject();
                        writer.WritePropertyName("name");
                        writer.WriteValue(dataItem.Name);

                        if (!String.IsNullOrEmpty(dataItem.Prompt))
                        {
                            writer.WritePropertyName("prompt");
                            writer.WriteValue(dataItem.Prompt);
                        }

                        if (!String.IsNullOrEmpty(dataItem.Value))
                        {
                            writer.WritePropertyName("value");
                            writer.WriteValue(dataItem.Value);
                        }
                        writer.WriteEndObject();
                    }
                    writer.WriteEndArray();
                }
                writer.WriteEndObject();
            }
            writer.WriteEndObject();
        }
    }
}