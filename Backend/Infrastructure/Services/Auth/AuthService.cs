using Application.Common.Helpers;
using Application.Common.Interfaces.JwtToken;
using Application.Common.Models;
using Application.Features.Auth;
using Application.Features.Auth.DTOs;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
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

        public async Task<TokenResult> LoginAsync(LoginRequestDto request)
        {
            var user = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserName == request.UserName);

            if (user is null)
                throw new UnauthorizedException("Invalid username or password.");

            var isValidPassword = _passwordHelper.VerifyPassword(
                user,
                request.Password,
                user.PasswordHash);

            if (!isValidPassword)
                throw new UnauthorizedException("Invalid username or password.");

            return _jwtTokenService.GenerateToken(user);
        }
    }
}
