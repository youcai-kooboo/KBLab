using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Framework.WebApi.Common;
using Framework.WebApi.Common.Configs;

namespace Framework.WebApi.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            ApiType apiType = ApiType.RootApi;

            //For Cross Domain Web API service call
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterWebApiFilters(GlobalConfiguration.Configuration.Filters, apiType);
            MessageHandlerConfig.RegisterMessageHandlers(GlobalConfiguration.Configuration.MessageHandlers, apiType);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FormatterConfig.RegisterFormatters(GlobalConfiguration.Configuration.Formatters);
            Bootstrapper.Run();

            //Database.SetInitializer(new AppSeedData());

            ////Enable when migration needed (After adding migration)
            //MigrationConfig.Update(); 
        }
    }
}