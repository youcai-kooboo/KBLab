using System.Net.Http;
using Framework.WebApi.Common;
using Framework.WebApi.Common.Assemblers;

namespace Framework.WebApi.Common.Handlers
{
    /// <summary>
    /// Global Context Handler
    /// Set context info and deal with HttpRequestMessage or HttpResponseMessage
    /// </summary>
    public class SetupContextHandler : SetupContextHandlerBase
    {
        private readonly IContextAssembler contextAssembler;

        public SetupContextHandler(IContextAssembler contextAssembler, ApiType apiType)
            : base( apiType)
        {
            this.contextAssembler = contextAssembler;
        }

        protected override void CreateContext(ApiContext context, HttpRequestMessage request)
        {
            contextAssembler.TranslateToContext(context, request, ApiType);
        }
    }
}