using Application.Common.Helpers;
using Application.Common.Interfaces.JwtToken;
using Application.Features.Auth;
using Application.Features.Auth.DTOs;
using Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;

namespace Infrastructure.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IPasswordHelper _passwordHelper;

        public AuthService(
            ApplicationDbContext dbContext,
            IJwtTokenService jwtTokenService,
            IPasswordHelper passwordHelper)
        {
            _dbContext = dbContext;
            _jwtTokenService = jwtTokenService;
            _passwordHelper = passwordHelper;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _dbContext.Users
                .Include(x => x.Employee)
                    .ThenInclude(x => x.JobTitle)
                .FirstOrDefaultAsync(x =>
                    x.UserName == request.UserName);

            if (user == null)
                throw new NotFoundException("Invalid username or password");

            var isValidPassword = _passwordHelper.VerifyPassword(user,
                request.Password,
                user.PasswordHash);

            if (!isValidPassword)
                throw new NotFoundException("Invalid username or password");



            var token = _jwtTokenService.GenerateToken(user);

            return new LoginResponseDto
            {
                AccessToken = token,
                ExpirationUtc = DateTime.UtcNow.AddHours(1)
            };
        }
    }
}
