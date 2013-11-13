using System;
using System.Web.Http.Filters;
using Framework.Data.Context;
using Framework.Data.Model;

namespace Framework.WebApi.Common.Filters
{
    public class LogExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ApiType apiType;

        public LogExceptionFilterAttribute(ApiType apiType)
        {
            this.apiType = apiType;
        }

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            using (AppContext context = new AppContext())
            {
                Exception ex = actionExecutedContext.Exception;
                context.ErrorLogs.Add(new ErrorLog
                    {
                        HelpLink = String.Format("{0};{1}", ApiContext.Current.Request.RequestUri, ex.HelpLink),
                        InnerException = ex.InnerException == null ? String.Empty : ex.InnerException.Message,
                        Message = String.Format("{0},{1}", apiType, ex.Message),
                        Source = ex.Source,
                        StackTrace = ex.StackTrace,
                        TimeOccured = DateTime.Now
                    });

                context.Commit();
            }
        }
    }
}
