using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;
using System.Linq;
using Framework.Data.Context;
using Framework.Data.Model;

namespace Framework.WebApi.Common.Filters
{
    public class AccessPermissionAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            HandleNoPermissionRequest(actionContext);
            
            base.OnAuthorization(actionContext);
        }

        protected void HandleNoPermissionRequest(HttpActionContext actionContext)
        {
            string controllerName = actionContext.ControllerContext.ControllerDescriptor.ControllerName.ToLower();
            HttpRequestMessage request = actionContext.Request;
            List<string> publicController = new List<string>() {"entrypage", "accesstoken"};

            if (!publicController.Contains(controllerName))
            {
                IEnumerable<string> accessToken;
                if (!request.Headers.TryGetValues("AccessToken", out accessToken))
                {
                    ModelStateDictionary modelStateDictionary = new ModelStateDictionary();
                    modelStateDictionary.AddModelError("Message", "An access token is required.");
 
                    actionContext.Response = request.CreateErrorResponse(HttpStatusCode.BadRequest, modelStateDictionary);
                }
                else
                {
                    using (AppContext context = new AppContext())
                    {
                        //validate the access token
                        ModelStateDictionary modelStateDictionary = new ModelStateDictionary();
                        AccessToken token = context.AccessTokens.FirstOrDefault(m => m.Token == accessToken.FirstOrDefault());
                        if (token == null)
                        {
                            modelStateDictionary.AddModelError("Message", "The access token is invalid.");
                            actionContext.Response = request.CreateErrorResponse(HttpStatusCode.BadRequest, modelStateDictionary);
                        }
                        else if (token.ExpirationDateUtc <= DateTime.Now)
                        {
                            modelStateDictionary.AddModelError("Message", "The access token is expired.");
                            actionContext.Response = request.CreateErrorResponse(HttpStatusCode.BadRequest,
                                                                                 modelStateDictionary);
                        }
                        else
                        {
                            //validate the controller
                            var controllers = token.Client.ClientScopes.Select(m => m.Scope.Controller.ToLower());
                            if (!controllers.Contains(controllerName))
                            {
                                modelStateDictionary.AddModelError("Message", "The service can't be accessed by the current access token.");
                                actionContext.Response = request.CreateErrorResponse(HttpStatusCode.BadRequest,
                                                                                     modelStateDictionary);
                            }
                        }
                    }
                }
            }
        }
    }
}
