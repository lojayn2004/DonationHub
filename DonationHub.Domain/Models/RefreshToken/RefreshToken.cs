using Microsoft.EntityFrameworkCore;

namespace DonationHub.Domain.Models.RefreshToken
{
    [Owned]
    public  class RefreshToken
    {
        public string Token { get; set; }

        public DateTime ExpiresAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? RevokedOn { get; set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        public bool IsActive => RevokedOn == null && !IsExpired;


    }
}
