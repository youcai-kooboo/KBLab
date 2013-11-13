using System;
using System.Web;
using System.Web.Routing;
using Framework.WebApi.Common.Extensions;

namespace Framework.WebApi.Common.Routing
{
    public class GuidConstraint : IRouteConstraint
    {
        #region IRouteConstraint Members

        public bool Match(HttpContextBase httpContext, Route route, string parameterName,
                          RouteValueDictionary values,
                          RouteDirection routeDirection)
        {
            if (values.ContainsKey(parameterName))
            {
                string stringValue = values[parameterName] as string;
                if (!String.IsNullOrEmpty(stringValue))
                {
                    return stringValue.IsGuid();
                }
            }
            return false;
        }

        #endregion
    }
}