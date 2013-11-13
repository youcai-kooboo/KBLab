using System.Collections.ObjectModel;
using System.Net.Http;
using Framework.WebApi.Common.Assemblers;
using Framework.WebApi.Common.Handlers;

namespace Framework.WebApi.Common.Configs
{
    public class MessageHandlerConfig
    {
        public static void RegisterMessageHandlers(Collection<DelegatingHandler> messageHandlers, ApiType apiType)
        {
            NotAcceptableHandler notAcceptableHandler = new NotAcceptableHandler();
            IContextAssembler contextAssembler = new ContextAssembler();
            SetupContextHandler setupContextHandler = new SetupContextHandler(contextAssembler, apiType);
            CrossDomainHandler crossDomainHandler = new CrossDomainHandler();

            messageHandlers.Add(notAcceptableHandler);
            messageHandlers.Add(setupContextHandler);
            messageHandlers.Add(crossDomainHandler);
        }
    }
}