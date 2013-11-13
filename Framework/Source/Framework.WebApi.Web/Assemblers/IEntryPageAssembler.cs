using System.Collections.Generic;
using Framework.WebApi.Web.Representations;
using Framework.WebApi.Web.Representations;
using Framework.WebApi.Web.Representations;

namespace Framework.WebApi.Web.Assemblers
{
    /// <summary>
    /// A proxy for the entry page objects.
    /// </summary>
    public interface IEntryPageAssembler
    {
        /// <summary>
        /// Translates a list of language codes into a <see cref="EntryPage"/> representation class.
        /// </summary>
        /// <returns>An instance of <see cref="EntryPage"/> containing the requested languages.</returns>
        EntryPage TranslateToEntryPage();
    }
}