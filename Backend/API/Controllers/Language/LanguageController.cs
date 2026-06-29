using API.Controllers.Common;
using Application.Common.Interfaces.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Language
{
    [ApiController]
    public class LanguageController : BaseApiController
    {
        private readonly ILanguageService _languageService;

        public LanguageController(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        [AllowAnonymous]
        [HttpGet("{culture}")]
        public IActionResult GetResources([FromRoute] string? culture = null)
        {
            var resources = _languageService.GetLanguageResources(culture);
            return Ok(resources);
        }
    }
}
