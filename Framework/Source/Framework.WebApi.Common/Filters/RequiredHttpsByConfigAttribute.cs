using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;
using Framework.WebApi.Common.Extensions;

namespace Framework.WebApi.Common.Filters
{
    public class RequiredHttpsByConfigAttribute : AuthorizationFilterAttribute
    {
        private const string ENABLE_SSL_SETTING = "EnableSSL";

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if(IsSslEnabled() && !actionContext.Request.IsHttpsHandledbyLB() && !IsRequestSecure(actionContext.Request))
            {
                HandleNonHttpsRequest(actionContext);
            }

            base.OnAuthorization(actionContext);
        }

        protected bool IsSslEnabled()
        {
            string enableSsl = ConfigurationManager.AppSettings[ENABLE_SSL_SETTING];

            return enableSsl != null && enableSsl.ToLower() == "true";
        }

        protected bool IsRequestSecure(HttpRequestMessage request)
        {
            return request.RequestUri.Scheme == Uri.UriSchemeHttps;
        }

        protected virtual void HandleNonHttpsRequest(HttpActionContext actionContext)
        {
            UriBuilder uri = new UriBuilder(actionContext.Request.RequestUri);
            uri.Scheme = Uri.UriSchemeHttps;
            uri.Port = 443;

            // Redirect GET to secure version
            if (actionContext.Request.Method.Equals(HttpMethod.Get))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Found);
                actionContext.Response.Headers.Location = uri.Uri;
            }
            // Otherwise the browser might not propagate the verb and request body correctly.
            else
            {
                ModelStateDictionary modelStateDictionary = new ModelStateDictionary();
                modelStateDictionary.AddModelError("Message", String.Format("A secure connection is required, The resource can be found at '{0}'.", uri.Uri.AbsoluteUri));
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.NotFound, modelStateDictionary);
            }
        }
    }
}
