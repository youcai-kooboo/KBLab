using System;
using System.Web.Http;
using System.Web.Mvc;

using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;
using Autofac.Integration.WebApi;

using Framework.Data.Infrastructure;
using Framework.Data.Repository;
using Framework.Data.Service;

namespace Framework.WebApi.Web
{
    public static class Bootstrapper
    {
        public static void Run()
        {
            SetAutofacWebAPI();
        }

        private static ContainerBuilder builder = null;
        private static IContainer container = null;

        private static void SetAutofacWebAPI()
        {
            builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerHttpRequest();
            builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerHttpRequest();

            builder.RegisterAssemblyTypes(typeof (ClientRepository).Assembly)
                   .Where(t => t.Name.EndsWith("Repository"))
                   .AsImplementedInterfaces().InstancePerHttpRequest();
            builder.RegisterAssemblyTypes(typeof (ClientService).Assembly)
                   .Where(t => t.Name.EndsWith("Service"))
                   .AsImplementedInterfaces().InstancePerHttpRequest();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                  .Where(t => t.Name.EndsWith("Assembler"))
                  .AsImplementedInterfaces().InstancePerHttpRequest();

            builder.RegisterFilterProvider();
            container = builder.Build();
 
            // Set the dependency resolver for Web API.
            var webApiResolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = webApiResolver;

            // Set the dependency resolver for MVC.
            var mvcResolver = new AutofacDependencyResolver(container);
            DependencyResolver.SetResolver(mvcResolver);
        }

        public static IContainer Container()
        {
            if (container != null) return container;
            throw new Exception("Container is not ready.");
        }
    }
}