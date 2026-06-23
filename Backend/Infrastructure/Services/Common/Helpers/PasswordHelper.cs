using Application.Common.Helpers;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.Common.Helpers
{
    public class PasswordHelper : IPasswordHelper
    {
        private readonly PasswordHasher<User> _passwordHasher;

        public PasswordHelper(IOptions<PasswordHasherOptions> options)
        {
            _passwordHasher = new PasswordHasher<User>(options);
        }

        public string HashPassword(User user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
        }

        public bool VerifyPassword(
            User user,
            string password,
            string passwordHash)
        {
            var result = _passwordHasher.VerifyHashedPassword(
                user,
                passwordHash,
                password);

            return result != PasswordVerificationResult.Failed;
        }
    }
}
