using API.Controllers.Common;
using Application.Features.Auth;
using Application.Features.Auth.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Shared.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
            var tokenResult = await _authService.LoginAsync(request, userAgent, timeZone, ct);

            var userContext = GetUserContextFromToken(tokenResult.AccessToken);
            var response = new LoginResponseDto
            {
                AccessToken = tokenResult.AccessToken,
                RefreshToken = tokenResult.RefreshToken,
                RequiresMfa = false,
                MfaToken = null,
                UserContext = userContext
            };

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
            var tokenResult = await _authService.RefreshTokenAsync(request, userAgent, timeZone, ct);

            var userContext = GetUserContextFromToken(tokenResult.AccessToken);
            var response = new LoginResponseDto
            {
                AccessToken = tokenResult.AccessToken,
                RefreshToken = tokenResult.RefreshToken,
                UserContext = userContext
            };

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

        private UserContextDto GetUserContextFromToken(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(accessToken);
            var claims = jwtToken.Claims;

            var id = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub || c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            var email = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email || c.Type == ClaimTypes.Email)?.Value ?? string.Empty;
            var name = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName || c.Type == ClaimTypes.Name)?.Value ?? string.Empty;
            var firstName = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value ?? string.Empty;
            var lastName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value ?? string.Empty;
            var roles = claims.Where(c => c.Type == ClaimTypes.Role || c.Type == "role").Select(c => c.Value).ToList();
            var permissions = claims.Where(c => c.Type == "permissions" || c.Type == "permission").Select(c => c.Value).ToList();

            return new UserContextDto
            {
                Id = id,
                UserName = name,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName,
                FullName = $"{firstName} {lastName}".Trim(),
                Roles = roles,
                Permissions = permissions
            };
        }
    }
}
