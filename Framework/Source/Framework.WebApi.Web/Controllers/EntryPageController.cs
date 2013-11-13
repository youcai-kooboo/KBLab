using System.Net;
using System.Net.Http;
using System.Web.Http;
using Framework.WebApi.Web.Assemblers;
using Framework.WebApi.Web.Representations;

namespace Framework.WebApi.Web.Controllers
{
    public class EntryPageController : ApiController
    {
        private readonly IEntryPageAssembler _entryPageAssembler;

        public EntryPageController(IEntryPageAssembler entryPageAssembler)
        {
            _entryPageAssembler = entryPageAssembler;
        }

        public HttpResponseMessage Get()
        {
            EntryPage entryPage = _entryPageAssembler.TranslateToEntryPage();
            HttpResponseMessage responseMessage = Request.CreateResponse(HttpStatusCode.OK, entryPage);

            return responseMessage;
        }
    }
}