using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DonationHub.Domain.Models.RefreshToken
{
    [Owned]
    public  class RefreshToken
    {
        public int Id { get; private set; }
        public string TokenHash { get; private set; }


        [NotMapped]
        public string? Token { get; private set; }

        public DateTime ExpiresAt { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime? RevokedOn { get; private set; }

        public int? ReplacedByTokenId { get; private set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        public bool IsActive => RevokedOn == null && !IsExpired;

        public static RefreshToken Create(string token, string tokenHash, DateTime createdAt, DateTime expiresAt)
        {
            return new RefreshToken()
            {
                Token = token,
                TokenHash = tokenHash,
                CreatedAt = createdAt,
                ExpiresAt = expiresAt

            };
        }

        public void RevokeToken(DateTime revokedOn, int? replacedByTokenId)
        {
            RevokedOn = revokedOn;
            if (replacedByTokenId is not null)
                ReplacedByTokenId = replacedByTokenId;

        }
    }
}
