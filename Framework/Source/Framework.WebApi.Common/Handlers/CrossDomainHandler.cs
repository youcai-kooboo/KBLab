using System;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Framework.WebApi.Common.Extensions;

namespace Framework.WebApi.Common.Handlers
{
    public class CrossDomainHandler : DelegatingHandler
    {
        private const string ORIGIN = "Origin";
        private const string ACCESS_CONTROL_ALLOW_ORIGIN = "Access-Control-Allow-Origin";
        private const string ACCESS_CONTRO_REQUEST_METHOD = "Access-Control-Request-Method";
        private const string ACCESS_CONTROL_REQUEST_HEADERS = "Access-Control-Request-Headers";
        private const string ACCESS_CONTROL_ALLOW_METHODS = "Access-Control-Allow-Methods";
        private const string ACCESS_CONTROL_ALLOW_HEADERS = "Access-Control-Allow-Headers";
        private const string DEFAULT_CONTENT_TYPE = "application/json";

        private const string JSONP_CALLBACK_QUERY_PARAMETER = "callback";
        private const string JSONP_TIMESTAMP_QUERY_PARAMETER = "_";

        [ExcludeFromCodeCoverage] // Unable to test as we cannot stub base.SendAsync
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                               CancellationToken cancellationToken)
        {
            if(!request.Headers.Accept.Any())
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypes.JSON_HAL));
            }

            bool isCorsRequest = request.Headers.Contains(ORIGIN);
            bool isPreflightedRequest = request.Method == HttpMethod.Options;
            if (isCorsRequest) // Is cross domain request
            {
                string requestOrigin = request.Headers.GetValues(ORIGIN).First();
                Uri requestOriginUri = new Uri(requestOrigin);
                if (!DomainInWhitelist(requestOriginUri.Authority)) // Domain not on a white-list disallow access.
                {
                    return base.SendAsync(request, cancellationToken);
                }
                else // Domain on a white-list allow access.
                {
                    if (isPreflightedRequest) // Preflighted cross domain request
                    {
                        return Task.Factory.StartNew(() => ProcessCorsPreflightedResponse(requestOrigin, request),
                                                     cancellationToken);
                    }
                    else // Actual cross domain request
                    {
                        if (request.Content.Headers.ContentType == null)
                        {
                            request.Content.Headers.ContentType = new MediaTypeHeaderValue(DEFAULT_CONTENT_TYPE);
                        }

                        return base.SendAsync(request, cancellationToken)
                            .ContinueWith(task => ProcessCorsResponse(requestOrigin, task.Result),
                                          TaskContinuationOptions.ExecuteSynchronously);
                    }
                }
            }
            else // Isn't cross domain request
            {
                return base.SendAsync(request, cancellationToken)
                    .ContinueWith(task => ProcessJsonpResponse(task.Result),
                                  TaskContinuationOptions.ExecuteSynchronously);
            }
        }

        /// <summary>
        /// Verify domain on a white-list
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        protected bool DomainInWhitelist(string domain)
        {
            return true;
            //List<string> domainsInSetting = new List<string>(){};
            //foreach (string domainInSetting in domainsInSetting)
            //{
            //    // domains can be added as text, like ('budgetair.nl') with wildcards for subdomain ('*.budgetair.nl')
            //    string domainExpression = String.Format("^{0}$", domainInSetting.Replace("*", "(.+)*"));
            //    Regex domainRegex = new Regex(domainExpression);
            //    if (domainRegex.IsMatch(domain))
            //    {
            //        return true;
            //    }
            //}
            //return false;
        }
 

        /// <summary>
        /// Preflighted cross domain request
        /// 
        /// The OPTIONS request you're seeing is because of preflighting - if you do a POST request 
        /// with custom headers or a content-type set to something other than application/x-www-form-urlencoded, 
        /// multipart/form-data, or text/plain, the browser attempts to make sure a request with those headers 
        /// will be allowed by making an OPTIONS request before it makes the actual POST request
        /// </summary>
        /// <param name="requestOrigin"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        protected HttpResponseMessage ProcessCorsPreflightedResponse(string requestOrigin, HttpRequestMessage request)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Headers.Add(ACCESS_CONTROL_ALLOW_ORIGIN, requestOrigin);

            string requestMethod = request.Headers.GetValues(ACCESS_CONTRO_REQUEST_METHOD).FirstOrDefault();
            if (!String.IsNullOrEmpty(requestMethod))
            {
                response.Headers.Add(ACCESS_CONTROL_ALLOW_METHODS, requestMethod);
            }

            string requestHeaders = String.Join(",", request.Headers.GetValues(ACCESS_CONTROL_REQUEST_HEADERS));
            if (!String.IsNullOrEmpty(requestHeaders))
            {
                response.Headers.Add(ACCESS_CONTROL_ALLOW_HEADERS, requestHeaders);
            }

            return response;
        }

        /// <summary>
        /// Actual cross domain request
        /// </summary>
        /// <param name="requestOrigin"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        protected HttpResponseMessage ProcessCorsResponse(string requestOrigin, HttpResponseMessage response)
        {
            response.Headers.Add(ACCESS_CONTROL_ALLOW_ORIGIN, requestOrigin);
            return response;
        }

        /// <summary>
        /// Keep the jsonp query string when there is a redirect
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        protected HttpResponseMessage ProcessJsonpResponse(HttpResponseMessage response)
        {
            if (response.Headers.Location != null) // Attach Jsonp Query Parameters
            {
                string location = response.Headers.Location.ToString();

                NameValueCollection queryStrings = HttpUtility.ParseQueryString(response.RequestMessage.RequestUri.Query);
                string callback = queryStrings[JSONP_CALLBACK_QUERY_PARAMETER];
                if (!String.IsNullOrEmpty(callback))
                {
                    location = location.AddQuery(JSONP_CALLBACK_QUERY_PARAMETER, callback);
                }

                string timestamp = queryStrings[JSONP_TIMESTAMP_QUERY_PARAMETER];
                if (!String.IsNullOrEmpty(timestamp))
                {
                    location = location.AddQuery(JSONP_TIMESTAMP_QUERY_PARAMETER, timestamp);
                }

                response.Headers.Location = new Uri(location);
            }

            return response;
        }
    }
}