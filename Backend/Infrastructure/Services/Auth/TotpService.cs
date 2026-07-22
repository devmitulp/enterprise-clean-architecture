using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Interfaces.Auth;
using Application.Common.Settings;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using OtpNet;
using QRCoder;

namespace Infrastructure.Services.Auth
{
    public class TotpService : ITotpService
    {
        private readonly IDataProtector _protector;
        private readonly MfaSettings _mfaSettings;

        public TotpService(
            IDataProtectionProvider dataProtectionProvider,
            IOptions<MfaSettings> mfaSettings)
        {
            _protector = dataProtectionProvider.CreateProtector("MfaSecretProtector");
            _mfaSettings = mfaSettings.Value;
        }

        public string GenerateSecretKey()
        {
            // 160-bit key (20 bytes) is standard for TOTP
            byte[] key = KeyGeneration.GenerateRandomKey(20);
            return Base32Encoding.ToString(key);
        }

        public string EncryptSecret(string secret)
        {
            if (string.IsNullOrEmpty(secret))
                throw new ArgumentNullException(nameof(secret));

            return _protector.Protect(secret);
        }

        public string DecryptSecret(string encryptedSecret)
        {
            if (string.IsNullOrEmpty(encryptedSecret))
                throw new ArgumentNullException(nameof(encryptedSecret));

            return _protector.Unprotect(encryptedSecret);
        }

        public string GenerateProvisioningUri(string email, string secret)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException(nameof(email));
            if (string.IsNullOrEmpty(secret))
                throw new ArgumentNullException(nameof(secret));

            var issuerEscaped = Uri.EscapeDataString(_mfaSettings.Issuer);
            var emailEscaped = Uri.EscapeDataString(email);

            return $"otpauth://totp/{issuerEscaped}:{emailEscaped}?secret={secret}&issuer={issuerEscaped}&algorithm=SHA1&digits=6&period=30";
        }

        public string GenerateQrCodeSvg(string provisioningUri)
        {
            if (string.IsNullOrEmpty(provisioningUri))
                throw new ArgumentNullException(nameof(provisioningUri));

            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(provisioningUri, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new SvgQRCode(qrCodeData);
            
            // Get graphic returns the raw SVG string
            return qrCode.GetGraphic(20, "#0F172A", "#FFFFFF", true);
        }

        public bool VerifyCode(string secret, string code)
        {
            if (string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(code))
                return false;

            try
            {
                byte[] secretBytes = Base32Encoding.ToBytes(secret);
                var totp = new Totp(secretBytes, step: 30, mode: OtpHashMode.Sha1, totpSize: 6);

                // Configurable clock skew: calculate steps based on MfaSettings
                int stepWindow = Math.Max(0, _mfaSettings.ClockSkewSeconds / 30);
                var verificationWindow = new VerificationWindow(previous: stepWindow, future: stepWindow);

                return totp.VerifyTotp(code, out _, verificationWindow);
            }
            catch
            {
                return false;
            }
        }

        public List<string> GenerateRecoveryCodes()
        {
            var codes = new List<string>();
            for (int i = 0; i < 8; i++)
            {
                // Generate secure random alphanumeric codes of 10 chars, in format XXXX-XXXX
                byte[] randomBytes = new byte[8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(randomBytes);
                }
                var rawCode = Convert.ToHexString(randomBytes).Substring(0, 8).ToLowerInvariant();
                var formattedCode = $"{rawCode.Substring(0, 4)}-{rawCode.Substring(4, 4)}";
                codes.Add(formattedCode);
            }
            return codes;
        }

        public string HashRecoveryCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentNullException(nameof(code));

            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(code.Trim().ToLowerInvariant()));
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }

        public bool VerifyRecoveryCode(string storedHash, string inputCode)
        {
            if (string.IsNullOrEmpty(storedHash) || string.IsNullOrEmpty(inputCode))
                return false;

            var hashedInput = HashRecoveryCode(inputCode);
            return string.Equals(storedHash, hashedInput, StringComparison.OrdinalIgnoreCase);
        }
    }
}
