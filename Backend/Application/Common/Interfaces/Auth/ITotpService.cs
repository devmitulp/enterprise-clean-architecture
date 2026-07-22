using System.Collections.Generic;

namespace Application.Common.Interfaces.Auth
{
    public interface ITotpService
    {
        string GenerateSecretKey();
        string EncryptSecret(string secret);
        string DecryptSecret(string encryptedSecret);
        string GenerateProvisioningUri(string email, string secret);
        string GenerateQrCodeSvg(string provisioningUri);
        bool VerifyCode(string secret, string code);
        List<string> GenerateRecoveryCodes();
        string HashRecoveryCode(string code);
        bool VerifyRecoveryCode(string storedHash, string inputCode);
    }
}
