using System.Collections.Generic;

namespace Application.Features.Auth.DTOs
{
    public class MfaSetupResponseDto
    {
        public string Secret { get; set; } = string.Empty;
        public string QrCodeSvg { get; set; } = string.Empty;
    }

    public class MfaEnableRequestDto
    {
        public string Code { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
    }

    public class MfaEnableResponseDto
    {
        public List<string> RecoveryCodes { get; set; } = new();
    }

    public class MfaDisableRequestDto
    {
        public string Code { get; set; } = string.Empty;
    }

    public class MfaVerifyRequestDto
    {
        public string Code { get; set; } = string.Empty;
        public string MfaToken { get; set; } = string.Empty;
        public string? Secret { get; set; }
    }
}
