using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace Framework.WebApi.Common.Extensions
{
    /// <summary>
    /// HttpRequest Extensions Class
    /// Combine the absolute url according to some parameters
    /// </summary>
    public static class HttpRequestExtensions
    {
        private const string REFERER = "Referer";
        private const string XREFERER = "XReferer";

        /// <summary>
        /// Combine the absolute url with the current request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string ToAbsoluteUrl(this HttpRequest request)
        {
            return ToAbsoluteUrl(request, request.RawUrl);
        }

        /// <summary>
        /// Combine the absolute url with the current request and relativeUrl
        /// </summary>
        /// <param name="request"></param>
        /// <param name="relativeUrl"></param>
        /// <returns></returns>
        public static string ToAbsoluteUrl(this HttpRequest request, string relativeUrl)
        {
            return ToAbsoluteUrl(request, request.Url.Authority, relativeUrl);
        }

        /// <summary>
        /// Combine the absolute url with the current request and host and relativeUrl
        /// </summary>
        /// <param name="request"></param>
        /// <param name="host"></param>
        /// <param name="relativeUrl"></param>
        /// <returns></returns>
        public static string ToAbsoluteUrl(this HttpRequest request, string host, string relativeUrl)
        {
            if(!relativeUrl.StartsWith("http://") && !relativeUrl.StartsWith("https://"))
            {
                return String.Format("{0}://{1}/{2}", request.Url.Scheme, host.TrimEnd('/'), relativeUrl.TrimStart('~', '/'));
            }

            return relativeUrl;
        }

        /// <summary>
        /// Combine the absolute url with the current request and relativeUrl
        /// </summary>
        /// <param name="request"></param>
        /// <param name="relativeUrl"></param>
        /// <returns></returns>
        public static string ToAbsoluteUrl(this HttpRequestMessage request, string relativeUrl)
        {
            return ToAbsoluteUrl(request, request.RequestUri.Authority, relativeUrl);
        }

        /// <summary>
        /// Combine the absolute url with the current request and relativeUrl
        /// </summary>
        /// <param name="request"></param>
        /// <param name="host"></param>
        /// <param name="relativeUrl"></param>
        /// <returns></returns>
        public static string ToAbsoluteUrl(this HttpRequestMessage request, string host, string relativeUrl)
        {
            if(!relativeUrl.StartsWith("http://") && !relativeUrl.StartsWith("https://"))
            {
                return String.Format("{0}://{1}/{2}", request.RequestUri.Scheme, host.TrimEnd('/'), relativeUrl.TrimStart('~', '/'));
            }

            return relativeUrl;
        }

        /// <summary>
        /// Retrieves the client IP address, whether it's in the remote-addr, http-forwarded, or x-forwarded-for headers.
        /// </summary>
        /// <param name="request">The HttpRequestMessage object</param>
        /// <returns></returns>
        public static string ClientIpAddress(this HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                var httpRequest = ((HttpContextBase)request.Properties["MS_HttpContext"]).Request;
                var reportedAddress = new List<String> { 
                    httpRequest.ServerVariables["HTTP_X_FORWARDED_FOR"], 
                    httpRequest.ServerVariables["HTTP_FORWARDED"], 
                    httpRequest.ServerVariables["REMOTE_ADDR"],
                    httpRequest.UserHostAddress
                }.FirstOrDefault(s => !String.IsNullOrWhiteSpace(s) && !String.IsNullOrEmpty(s));
                return reportedAddress;
            }

            return null;
        }

        public static bool IsHttpsHandledbyLB(this HttpRequestMessage request)
        {
            return request.Headers.Contains("SSL-Handledby-LB");
        }

        public static Uri CreateLocation(this HttpRequestMessage request, UriPartial part, string virtualPath)
        {
            return CreateLocation(request, request.RequestUri.GetLeftPart(part).CombineVirtualPath(virtualPath));
        }

        public static Uri CreateLocation(this HttpRequestMessage request, string uriString)
        {
            Uri location = new Uri(uriString);
            if(request.IsHttpsHandledbyLB())
            {
                location = location.ToHttps();
            }

            return location;
        }

        public static void Setup(this HttpRequestMessage request)
        {
            HttpRequestHeaders headers = request.Headers;

            NameValueCollection queryString = request.RequestUri.ParseQueryString();
            if (headers.Referrer == null)
            {
                string referer = queryString[REFERER];
                if (!String.IsNullOrEmpty(referer))
                {
                    headers.Add("Referer", referer);
                }
            }

            IEnumerable<string> xRefereres = null;
            if (!headers.TryGetValues("X-Referer", out xRefereres))
            {
                headers.Add("X-Referer", queryString[XREFERER]);
            }

            IEnumerable<string> ipAddresses = null;
            if (headers.TryGetValues("X-Forwarded-For", out ipAddresses))
            {
                headers.Add("X-Forwarded-For", ipAddresses.FirstOrDefault());
            }
            else
            {
                headers.Add("X-Forwarded-For", request.ClientIpAddress());
            }
        }
    }
}