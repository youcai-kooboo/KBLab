using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Framework.WebApi.Common;
using Newtonsoft.Json.Converters;

namespace Framework.WebApi.Hal
{
    public class JsonHalMediaTypeFormatter : JsonMediaTypeFormatter
    {
        private const string JSONP_CALLBACK_QUERY_PARAMETER = "callback";
        private readonly HttpRequestMessage _request;

        public JsonHalMediaTypeFormatter()
        {
            SupportedMediaTypes.Clear();
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(MediaTypes.JSON_HAL));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(MediaTypes.JAVASCRIPT));

            SerializerSettings.Converters.Add(new LinksConverter());
            SerializerSettings.Converters.Add(new HintsConverter());
            SerializerSettings.Converters.Add(new RepresentationConverter());
            SerializerSettings.Converters.Add(new StringEnumConverter());
        }

        public JsonHalMediaTypeFormatter(HttpRequestMessage request)
            : this()
        {
            _request = request;
        }

        public override bool CanReadType(Type type)
        {
            return typeof (IRepresentation).IsAssignableFrom(type);
        }

        public override bool CanWriteType(Type type)
        {
            return typeof (IRepresentation).IsAssignableFrom(type);
        }

        public override MediaTypeFormatter GetPerRequestFormatterInstance(Type type, HttpRequestMessage request,
                                                                          MediaTypeHeaderValue mediaType)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (request == null)
                throw new ArgumentNullException("request");

            return new JsonHalMediaTypeFormatter(request) {SerializerSettings = SerializerSettings};
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream stream, HttpContent content,
                                                TransportContext transportContext)
        {
            string callback;
            if (IsJsonpRequest(_request, out callback))
            {
                StreamWriter writer = new StreamWriter(stream);
                writer.Write(callback + "(");
                writer.Flush();

                return base.WriteToStreamAsync(type, value, stream, content, transportContext)
                    .ContinueWith(_ =>
                                      {
                                          writer.Write(")");
                                          writer.Flush();
                                      });
            }

            return base.WriteToStreamAsync(type, value, stream, content, transportContext);
        }

        private bool IsJsonpRequest(HttpRequestMessage request, out string callback)
        {
            callback = null;
            if (request == null || request.Method != HttpMethod.Get)
            {
                return false;
            }

            NameValueCollection query = HttpUtility.ParseQueryString(request.RequestUri.Query);
            callback = query[JSONP_CALLBACK_QUERY_PARAMETER];

            return !String.IsNullOrEmpty(callback);
        }
    }
}