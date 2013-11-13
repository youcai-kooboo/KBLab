using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;

namespace Framework.WebApi.Common.Extensions
{
    public static class HttpClientExtensions
    {
        private const string REFERER = "Referer";
        private const string XREFERER = "XReferer";

        public static void Setup(this HttpClient client, HttpRequestMessage request, bool attachXReferer = true)
        {
            IEnumerable<string> ipAddresses = null;
            if (request.Headers.TryGetValues("X-Forwarded-For", out ipAddresses))
            {
                client.DefaultRequestHeaders.Add("X-Forwarded-For", ipAddresses.FirstOrDefault());
            }
            else
            {
                client.DefaultRequestHeaders.Add("X-Forwarded-For", request.ClientIpAddress());
            }

            NameValueCollection queryString = request.RequestUri.ParseQueryString();
            if (request.Headers.Referrer != null)
            {
                client.DefaultRequestHeaders.Add("Referer", request.Headers.Referrer.ToString());
            }
            else
            {
                string referer = queryString[REFERER];
                if (!String.IsNullOrEmpty(referer))
                {
                    client.DefaultRequestHeaders.Add("Referer", referer);
                }
            }

            if (attachXReferer)
            {
                IEnumerable<string> xRefereres = null;
                if (request.Headers.TryGetValues("X-Referer", out xRefereres))
                {
                    client.DefaultRequestHeaders.Add("X-Referer", xRefereres.FirstOrDefault());
                }
                else
                {
                    client.DefaultRequestHeaders.Add("X-Referer", queryString[XREFERER]);
                }
            }

            // UserAgent and AcceptLanguage may be empty collection but never null, so we don't need to check if it is null
            client.DefaultRequestHeaders.Add("Accept-Language", request.Headers.AcceptLanguage.ToString());

            client.DefaultRequestHeaders.UserAgent.TryParseAdd(request.Headers.UserAgent.ToString());
        }
    }
}