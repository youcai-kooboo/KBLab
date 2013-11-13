using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Framework.WebApi.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "Controller",
                routeTemplate: "{controller}/{id}",
                defaults: new {controller = "EntryPage", id = RouteParameter.Optional });
        }
    }
}