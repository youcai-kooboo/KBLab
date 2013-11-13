using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Framework.WebApi.Common.Extensions;

namespace Framework.WebApi.Common.Handlers
{
    [ExcludeFromCodeCoverage]
    public class NotAcceptableHandler : DelegatingHandler
    {
        private const char SEPARATOR = ';';

        private string[] supportedMediaTypes = new string[]
                                                   {
                                                       MediaTypes.ANY,
                                                       MediaTypes.HTML,
                                                       MediaTypes.JSON,
                                                       MediaTypes.JSON_HAL,
                                                       MediaTypes.JAVASCRIPT,
                                                       MediaTypes.IMAGE
                                                   };

        [ExcludeFromCodeCoverage] // Unable to test as we cannot stub base.SendAsync
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                               CancellationToken cancellationToken)
        {
            // API does not support the requested media-type, a 406 – Not Acceptable response should be returned
            IEnumerable<string> accepts = null;
            if (request.Headers.TryGetValues("Accept", out accepts))
            {
                bool acceptsAreSupported = accepts.Any(accept => accept.SplitRemoveEmptyEntries(SEPARATOR).Any(it => supportedMediaTypes.Contains(it, StringComparer.OrdinalIgnoreCase)));
                if (!acceptsAreSupported)
                {
                    return Task<HttpResponseMessage>.Factory.StartNew(
                        () => new HttpResponseMessage(HttpStatusCode.NotAcceptable));
                }
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}