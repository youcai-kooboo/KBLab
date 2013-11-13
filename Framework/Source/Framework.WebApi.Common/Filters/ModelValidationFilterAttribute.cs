using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;

namespace Framework.WebApi.Common.Filters
{
    public class ModelValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                ModelStateDictionary modelState = new ModelStateDictionary();
                foreach (KeyValuePair<string, ModelState> keyValue in actionContext.ModelState)
                {
                    foreach (ModelError error in keyValue.Value.Errors)
                    {
                        string errorMessage = error.Exception != null ? error.Exception.Message : error.ErrorMessage;
                        modelState.AddModelError(keyValue.Key, errorMessage);
                    }
                }
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, modelState);
            }
        }
    }
}