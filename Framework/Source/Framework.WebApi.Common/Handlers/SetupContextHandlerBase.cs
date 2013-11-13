using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.WebApi.Common.Handlers
{
    /// <summary>
    /// Base class for setting up context. Including Session and Context management and Logging of requests and responses
    /// </summary>
    public abstract class SetupContextHandlerBase : DelegatingHandler
    {
        protected ApiType ApiType { get; private set; }

        protected abstract void CreateContext(ApiContext context, HttpRequestMessage request);

        protected SetupContextHandlerBase(ApiType apiType)
        {
            this.ApiType = apiType;
        }

        [ExcludeFromCodeCoverage] // Unable to test as we cannot stub base.SendAsync
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Task<HttpResponseMessage> processTask;

            // First create the context
            try
            {
                CreateContext(ApiContext.Current, request);

                // Let the base handle the request
                processTask = base.SendAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                TaskCompletionSource<HttpResponseMessage> taskCompletionSource = new TaskCompletionSource<HttpResponseMessage>();
                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                taskCompletionSource.SetResult(response);
                processTask = taskCompletionSource.Task;
            }
            
            // After request was handled, dispose the context and session
            processTask = processTask.ContinueWith(task => DisposeSessionAndContext(task.Result),
                                                            TaskContinuationOptions.ExecuteSynchronously);

            return processTask;
        }

        protected HttpResponseMessage DisposeSessionAndContext(HttpResponseMessage response)
        {
            ApiContext.Current.Dispose();

            return response;
        }
    }
}