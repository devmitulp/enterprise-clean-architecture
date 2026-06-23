using Application.Common.Contexts;
using Application.Common.Interfaces.Auth;

namespace Infrastructure.Services.Auth
{
    public class CurrentUserContext : ICurrentUserContext
    {
        public int? UserId => UserContext.UserId;
        public string? UserName => UserContext.UserName;
    }
}
