using System.Security.Cryptography;

namespace DonationHub.Infrastructure.Implementations
{
    internal static class RefreshTokenUtils
    {
        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public static string HashRefreshToken(string refreshToken)
        {
            var tokenBytes = System.Text.Encoding.UTF8.GetBytes(refreshToken);
            var hashBytes = SHA256.HashData(tokenBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
