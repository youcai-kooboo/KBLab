using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace Framework.WebApi.Hal
{
    public class RepresentationConverter : JsonConverter
    {
        private const string LINKS_PROPERTY_NAME = "_links";
        private const string EMBEDDED_PROPERTY_NAME = "_embedded";

        public override bool CanConvert(Type objectType)
        {
            return typeof(IRepresentation).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        JsonSerializer serializer)
        {
            return reader.Value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Representation representation = (Representation)value;

            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteStartObject();

            // Serialize links
            if (representation.Links.Any())
            {
                writer.WritePropertyName(LINKS_PROPERTY_NAME);
                serializer.Serialize(writer, representation.Links);
            }

            // Serialize hints
            serializer.Serialize(writer, representation.Hints);

            // Serialize embedded and the other properties
            Type baseRepresentationType = typeof(Representation);
            Type representationType = representation.GetType();
            IEnumerable<PropertyInfo> representationProperties = representationType.GetProperties();

            IEnumerable<PropertyInfo> embeddedRepresentationCollectionProperties = GetEmbeddedRepresentationCollectionProperties(baseRepresentationType, representationProperties);
            IEnumerable<PropertyInfo> embeddedRepresentationProperties = GetEmbeddedRepresentationProperties(baseRepresentationType, representationProperties, representation);
            if (embeddedRepresentationCollectionProperties.Any() || embeddedRepresentationProperties.Any())
            {
                writer.WritePropertyName(EMBEDDED_PROPERTY_NAME);

                writer.WriteStartObject();
                foreach (PropertyInfo embeddedRepresentationCollectionProperty in embeddedRepresentationCollectionProperties)
                {
                    ICollection embeddedRepresentationCollectionPropertyValue = (ICollection)embeddedRepresentationCollectionProperty.GetValue(value, null);

                    writer.WritePropertyName(embeddedRepresentationCollectionProperty.Name);
                    writer.WriteStartArray();
                    if (embeddedRepresentationCollectionPropertyValue != null)
                    {
                        foreach (object embeddedRepresentation in embeddedRepresentationCollectionPropertyValue)
                        {
                            WriteJson(writer, embeddedRepresentation, serializer);
                        }
                    }
                    writer.WriteEndArray();
                }

                foreach (PropertyInfo embeddedRepresentationProperty in embeddedRepresentationProperties)
                {
                    Representation embeddedRepresentationPropertyValue = (Representation)embeddedRepresentationProperty.GetValue(value, null);

                    writer.WritePropertyName(embeddedRepresentationProperty.Name);
                    WriteJson(writer, embeddedRepresentationPropertyValue, serializer);
                }
                writer.WriteEndObject();
            }

            IEnumerable<string> excludePropertyNames = baseRepresentationType.GetProperties().Select(it => it.Name)
                .Concat(embeddedRepresentationCollectionProperties.Select(it => it.Name))
                .Concat(embeddedRepresentationProperties.Select(it => it.Name));
            IEnumerable<PropertyInfo> properties =
                representationProperties.Where(it => !excludePropertyNames.Contains(it.Name));
            foreach (PropertyInfo property in properties)
            {
                writer.WritePropertyName(property.Name);
                object propertyValue = property.GetValue(value, null);
                serializer.Serialize(writer, propertyValue);
            }

            writer.WriteEndObject();
        }

        protected IEnumerable<PropertyInfo> GetEmbeddedRepresentationCollectionProperties(
            Type baseRepresentationType,
            IEnumerable<PropertyInfo> representationProperties)
        {
            Type collectionType = typeof(ICollection);
            return
                representationProperties.Where(
                    representationProperty =>
                    representationProperty.PropertyType.IsGenericType &&
                    collectionType.IsAssignableFrom(representationProperty.PropertyType) &&
                   baseRepresentationType.IsAssignableFrom(representationProperty.PropertyType.GetGenericArguments()[0]) &&
                   !representationProperty.PropertyType.GetGenericArguments()[0].IsDefined(typeof(NonEmbeddedAttribute), false));
        }

        protected IEnumerable<PropertyInfo> GetEmbeddedRepresentationProperties(
            Type baseRepresentationType,
            IEnumerable<PropertyInfo> representationProperties, Representation representation)
        {
            foreach (PropertyInfo representationProperty in representationProperties)
            {
                if (representationProperty.PropertyType.IsDefined(typeof (NonEmbeddedAttribute), false))
                {
                    continue;
                }

                if (representationProperty.PropertyType.IsInterface)
                {
                    object representationPropertyValue = representationProperty.GetValue(representation, null);
                    if (baseRepresentationType.IsInstanceOfType(representationPropertyValue))
                    {
                        yield return representationProperty;
                    }
                }
                else if (baseRepresentationType.IsAssignableFrom(representationProperty.PropertyType))
        {
                    yield return representationProperty;
                }
            }
        }
    }
}