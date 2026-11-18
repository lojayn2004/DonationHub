using System.Text.Json.Serialization;

namespace DonationHub.Application.Features.Auth.Common
{
    public class AuthResponse
    {
        public string Token { get; set; } = "";

        [JsonIgnore]
        public string? RefreshToken { get; set; } = "";

        public DateTimeOffset RefreshTokenExpiration { get; set; }
    }
}
