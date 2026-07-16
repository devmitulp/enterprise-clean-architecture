using API.Controllers.Common;
using Application.Features.Auth;
using Application.Features.Auth.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shared.Constants;

namespace API.Controllers.Auth
{
    [ApiController]
    [Tags("Auth")]
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [EnableRateLimiting(RateLimitPolicies.Login)]
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequestDto request,
            CancellationToken ct)
        {
            var userAgent = Request.Headers.UserAgent.ToString();
            var timeZone = Request.Headers["X-Timezone"].ToString();
            var response = await _authService.LoginAsync(request, userAgent, timeZone, ct);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(
            [FromBody] RefreshTokenRequestDto request,
            CancellationToken ct)
        {
            var userAgent = Request.Headers.UserAgent.ToString();
            var timeZone = Request.Headers["X-Timezone"].ToString();
            var response = await _authService.RefreshTokenAsync(request, userAgent, timeZone, ct);

            return Ok(response);
        }

        [HttpPost("logout")]
        [ProducesResponseType(typeof(Shared.Results.Result), StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout(
            [FromBody] LogoutRequestDto request,
            CancellationToken ct)
        {
            var response = await _authService.LogoutAsync(request, ct);
            if (!response.Succeeded)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
