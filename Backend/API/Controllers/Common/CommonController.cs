using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Common
{
    [Authorize]
    [ApiController]
    public class CommonController : BaseApiController
    {
        [AllowAnonymous]
        [HttpGet("settings")]
        public IActionResult GetSettings()
        {
            return Ok();
        }
    }
}
