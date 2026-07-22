using System;
using System.Collections.Generic;

namespace Application.Features.Auth.DTOs
{
    public class LoginResponseDto
    {
        public string? AccessToken { get; set; }

        public string? RefreshToken { get; set; }

        public bool RequiresMfa { get; set; }

        public bool IsMfaSetupRequired { get; set; }

        public string? MfaToken { get; set; }

        public string? QrCodeSvg { get; set; }

        public string? Secret { get; set; }

        public UserContextDto? UserContext { get; set; }
    }

    public class UserContextDto
    {
        public string Id { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string EmailAddress { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public List<string> Roles { get; set; } = new();

        public List<string> Permissions { get; set; } = new();

        public string? TenantId { get; set; }

        public string? Department { get; set; }

        public bool IsMfaEnabled { get; set; }
    }
}
