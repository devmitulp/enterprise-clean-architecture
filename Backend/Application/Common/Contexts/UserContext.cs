
namespace Application.Common.Contexts
{
    public static class UserContext
    {
        private static readonly AsyncLocal<int?> _currentUserId = new();
        private static readonly AsyncLocal<string?> _currentUserName = new();

        public static int? UserId
        {
            get => _currentUserId.Value;
            set => _currentUserId.Value = value;
        }

        public static string? UserName
        {
            get => _currentUserName.Value;
            set => _currentUserName.Value = value;
        }
    }
}
