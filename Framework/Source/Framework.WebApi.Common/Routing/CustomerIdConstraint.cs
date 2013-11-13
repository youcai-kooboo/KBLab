using System;
using System.Web;
using System.Web.Http.Routing;
using System.Web.Routing;
using Framework.WebApi.Common.Extensions;

namespace Framework.WebApi.Common.Routing
{
    public class CustomerIdConstraint : IRouteConstraint
    {
        protected const string CUSTOMERID_PREFIX = "cid-";
        protected const string CUSTOMERID = "customerId";

        public bool Match(HttpContextBase httpContext, Route route, string parameterName,
                          RouteValueDictionary values,
                          RouteDirection routeDirection)
        {
            if (values.ContainsKey(parameterName))
            {
                string stringValue = values[parameterName] as string;
                if (!String.IsNullOrEmpty(stringValue))
                {
                    if (stringValue.StartsWith(CUSTOMERID_PREFIX, StringComparison.OrdinalIgnoreCase))
                    {
                        string customerId = stringValue.TrimStart(CUSTOMERID_PREFIX, StringComparison.OrdinalIgnoreCase);
                        return customerId.IsGuid();
                    }
                }
            }
            return false;
        }

        public static string ExtractCustomerIdFromRouteData(IHttpRouteData routeData)
        {
            if (routeData != null)
            {
                object customerIdFromRoute = null;
                routeData.Values.TryGetValue(CUSTOMERID, out customerIdFromRoute);
                if (customerIdFromRoute != null)
                {
                    return customerIdFromRoute
                        .ToString()
                        .TrimStart(CUSTOMERID_PREFIX, StringComparison.OrdinalIgnoreCase);
                }
            }

            return String.Empty;
        }

        public static string ConvertCustomerIdToRouteValue(string customerId)
        {
            return String.Concat(CUSTOMERID_PREFIX, customerId);
        }
    }
}