namespace Application.Common.Interfaces.Auth
{
    public interface ICurrentUserContext
    {
        int? UserId { get; }
        string? UserName { get; }
    }
}
