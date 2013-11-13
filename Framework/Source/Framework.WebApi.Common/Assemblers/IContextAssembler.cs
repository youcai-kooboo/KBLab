using System.Net.Http;
using Framework.WebApi.Common;

namespace Framework.WebApi.Common.Assemblers
{
    public interface IContextAssembler
    {
        void TranslateToContext(ApiContext context, HttpRequestMessage request, ApiType apiType);
    }
}