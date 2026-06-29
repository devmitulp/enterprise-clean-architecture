using System.Collections.Generic;

namespace Application.Common.Interfaces.Localization
{
    public interface ILocalizationService
    {
        string L(string key, params object[] args);

        IDictionary<string, string> GetResources(string? culture = null);
    }
}
