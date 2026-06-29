using System.Collections.Generic;

namespace Application.Common.Interfaces.Localization
{
    public interface ILanguageService
    {
        IDictionary<string, string> GetLanguageResources(string? culture = null);
    }
}
