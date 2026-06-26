using Application.Common.Interfaces.Auth;

namespace Infrastructure.Services.Auth
{
    public class UserContext : IUserContext
    {
        public int? UserId { get; set; }
        public string? UserName { get; set; }
    }
}
