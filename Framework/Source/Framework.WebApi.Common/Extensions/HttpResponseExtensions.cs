using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Framework.WebApi.Common.Routing;

namespace Framework.WebApi.Common.Extensions
{
    public static class HttpResponseExtensions
    {
        public static void SetCacheControlHeader(this HttpResponseMessage response, CacheControlHeaderValue cacheControlHeader)
        {
            HttpRequestMessage request = response.RequestMessage;
            if (request != null)
            {
                string customerId = CustomerIdConstraint.ExtractCustomerIdFromRouteData(response.RequestMessage.GetRouteData());
                if (!String.IsNullOrEmpty(customerId))
                {
                    // If the API creates a cid and returns that in the representation then that response should not be cache-able.
                    response.Headers.CacheControl = cacheControlHeader;
                }
            }
        }
    }
}