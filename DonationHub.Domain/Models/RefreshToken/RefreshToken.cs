
using System.ComponentModel.DataAnnotations.Schema;

namespace DonationHub.Domain.Models.RefreshToken
{

    public  class RefreshToken
    {
        public Guid Id { get; private set; }

        public Guid UserId { get; private set; }
        public string TokenHash { get; private set; }

        public string? Token { get; private set; }

        public DateTimeOffset ExpiresAt { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }

        public DateTimeOffset? RevokedOn { get; private set; }

        public Guid? ReplacedByTokenId { get; private set; }

        public bool IsExpired => DateTimeOffset.UtcNow >= ExpiresAt;

        public bool IsActive => RevokedOn == null && !IsExpired;

        public static RefreshToken Create(string token, 
            string tokenHash, DateTimeOffset 
            createdAt, DateTimeOffset expiresAt)
        {
            return new RefreshToken()
            {
                Token = token,
                TokenHash = tokenHash,
                CreatedAt = createdAt,
                ExpiresAt = expiresAt

            };
        }

        public void RevokeToken(DateTimeOffset revokedOn, Guid? replacedByTokenId)
        {
            RevokedOn = revokedOn;
            if (replacedByTokenId is not null)
                ReplacedByTokenId = replacedByTokenId;

        }
    }
}
