using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Formatting;
using Framework.WebApi.Hal;
using Newtonsoft.Json.Converters;

namespace Framework.WebApi.Web
{
    public class FormatterConfig
    {
        public static void RegisterFormatters(MediaTypeFormatterCollection formatters)
        {
            // Remove Xml Formatter
            formatters.Remove(formatters.XmlFormatter);

            //Configure default json formatter
            JsonMediaTypeFormatter jsonFormatter = formatters.JsonFormatter;
            jsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());
            jsonFormatter.SerializerSettings.Converters.Add(new LinksConverter());

            // Add Json Hal Formatter as default media type
            formatters.Insert(0, new JsonHalMediaTypeFormatter());
        }
    }
}