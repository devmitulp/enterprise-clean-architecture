using Application.Common.Helpers;
using Application.Common.Interfaces.JwtToken;
using Application.Common.Interfaces.Persistence;
using Application.Common.Models;
using Application.Features.Auth;
using Application.Features.Auth.DTOs;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;

namespace Infrastructure.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IPasswordHelper _passwordHelper;

        public AuthService(
            IRepository<User> userRepository,
            IJwtTokenService jwtTokenService,
            IPasswordHelper passwordHelper)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
            _passwordHelper = passwordHelper;
        }

        public async Task<TokenResult> LoginAsync(
            LoginRequestDto request,
            CancellationToken ct = default)
        {
            var user = await _userRepository
                        .AsQueryable()
                        .Include(x => x.Employee)
                        .ThenInclude(x => x.JobTitle)
                        .Where(x => x.UserName == request.UserName && x.IsActive)
                        .FirstOrDefaultAsync(ct);

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
