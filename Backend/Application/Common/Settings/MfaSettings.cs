namespace Application.Common.Settings
{
    public class MfaSettings
    {
        public string Issuer { get; set; } = "EnterpriseCleanArch";
        public int ClockSkewSeconds { get; set; } = 30;
        public int MaxFailedAttempts { get; set; } = 5;
        public int LockoutMinutes { get; set; } = 15;
    }
}
