using System.Collections.Generic;
using Application.Common.Interfaces.Localization;

namespace Infrastructure.Services.Common.Localization
{
    public class LanguageService : ILanguageService
    {
        private readonly ILocalizationService _localizationService;

        public LanguageService(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        public IDictionary<string, string> GetLanguageResources()
        {
            return _localizationService.GetResources();
        }
    }
}
