using System.Net.Http;
using Framework.WebApi.Common.Extensions;

namespace Framework.WebApi.Common.Assemblers
{
    public class ContextAssembler : IContextAssembler
    {
        public ContextAssembler()
        {
        }

        public void TranslateToContext(ApiContext context, HttpRequestMessage request, ApiType apiType)
        {
            string domain = request.RequestUri.GetDomain();
            context.Domain = domain;
            context.Request = request;
        }
    }
}