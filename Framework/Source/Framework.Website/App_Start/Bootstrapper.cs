using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;
using Framework.Data.Repository;
using Framework.Data.Service;
using Framework.Website.Mappings;
using Framework.Data.Infrastructure;

namespace Framework.Website
{
    public static class Bootstrapper
    {
        public static void Run()
        {
            SetAutofacContainerForMvcWeb();

            //Configure AutoMapper
           AutoMapperConfiguration.Configure();
        }

        private static void SetAutofacContainerForMvcWeb()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerHttpRequest();

            builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerHttpRequest();

            builder.RegisterAssemblyTypes(typeof (UserProfileRepository).Assembly)
                   .Where(t => t.Name.EndsWith("Repository"))
                   .AsImplementedInterfaces().InstancePerHttpRequest();

            builder.RegisterAssemblyTypes(typeof (UserProfileService).Assembly)
                   .Where(t => t.Name.EndsWith("Service"))
                   .AsImplementedInterfaces().InstancePerHttpRequest();
          
            builder.RegisterFilterProvider();
            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}