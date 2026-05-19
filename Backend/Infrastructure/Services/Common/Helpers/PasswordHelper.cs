using Application.Common.Helpers;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services.Common.Helpers
{
    public class PasswordHelper : IPasswordHelper
    {
        private readonly PasswordHasher<User> _passwordHasher;

        public PasswordHelper()
        {
            _passwordHasher = new PasswordHasher<User>();
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
