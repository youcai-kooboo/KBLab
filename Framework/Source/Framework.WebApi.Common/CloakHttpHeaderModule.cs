using System;
using System.Diagnostics.CodeAnalysis;
using System.Web;

namespace Framework.WebApi.Common
{
    [ExcludeFromCodeCoverage] // Unable to test this class as HttpContext cannot be mocked
    public class CloakHttpHeaderModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += OnPreSendRequestHeaders;
        }

        public void Dispose()
        {
        }

        private void OnPreSendRequestHeaders(object sender, EventArgs e)
        {
            if (HttpContext.Current != null)
            {
                try
                {
                    HttpContext.Current.Response.Headers.Remove("Server");
                }
                catch (PlatformNotSupportedException)
                {
                    // The operation requires the integrated pipeline mode in IIS 7.0 and at least the .NET Framework version 3.0.
                }
            }
        }
    }
}