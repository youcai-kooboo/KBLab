using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

[assembly: System.Web.PreApplicationStartMethod(typeof(Framework.WebApi.Common.ApiStartup), "Start")]

namespace Framework.WebApi.Common
{
    [ExcludeFromCodeCoverage] // Unable to test this class as DynamicModuleUtility and MvcHandler cannot be mocked
    public class ApiStartup
    {
        public static void Start()
        {
            // Removing technical and detailed version information from the HTTP header.
            DynamicModuleUtility.RegisterModule(typeof (CloakHttpHeaderModule));
            MvcHandler.DisableMvcResponseHeader = true;
        }
    }
}