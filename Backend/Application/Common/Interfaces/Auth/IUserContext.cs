namespace Application.Common.Interfaces.Auth
{
    public interface IUserContext
    {
        int? UserId { get; }
        string? UserName { get; }
    }
}
