using Framework.Data.Service;
using Framework.WebApi.Common;
using Framework.WebApi.Web.Controllers;
using Framework.WebApi.Web.Representations;
using Framework.WebApi.Web.Services;

namespace Framework.WebApi.Web.Assemblers
{
    public class EntryPageAssembler : IEntryPageAssembler
    {
        private readonly ApiContext _context;
        public EntryPageAssembler(IAccessTokenService accessTokenService)
        {
            _context = ApiContext.Current;
        }

        public EntryPage TranslateToEntryPage()
        {
            EntryPage entryPage = new EntryPage();

            entryPage.AddLink(typeof (EntryPageController), _context);

            return entryPage;
        }
    }
}