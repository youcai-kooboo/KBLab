using System.Web.Http.Filters;
using Framework.WebApi.Common.Filters;

namespace Framework.WebApi.Common.Configs
{
    public class FilterConfig
    {
        public static void RegisterWebApiFilters(HttpFilterCollection filters, ApiType apiType)
        {            
            ModelValidationFilterAttribute modelValidationFilterAttribute = new ModelValidationFilterAttribute();
            filters.Add(modelValidationFilterAttribute);

            LogExceptionFilterAttribute logExceptionFilterAttribute = new LogExceptionFilterAttribute(apiType);
            filters.Add(logExceptionFilterAttribute);

            RequiredHttpsByConfigAttribute requiredHttpsByConfigAttribute = new RequiredHttpsByConfigAttribute();
            filters.Add(requiredHttpsByConfigAttribute);

            //AccessPermissionAttribute accessPermissionAttribute = new AccessPermissionAttribute();
            //filters.Add(accessPermissionAttribute);
        }
    }
}