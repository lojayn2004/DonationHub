using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DonationHub.Domain.Models.RefreshToken
{
    [Owned]
    public  class RefreshToken
    {
    public int Id { get; set; }
        public string TokenHash { get; set; }


        [NotMapped]
        public string? Token { get; set; }

        public DateTime ExpiresAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? RevokedOn { get; set; }

        public int? ReplacedByTokenId { get; set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        public bool IsActive => RevokedOn == null && !IsExpired;


    }
}
