using System.ComponentModel.DataAnnotations;

namespace DonationHub.Shared.Options 
{ 
    public class DonationOptions
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "Minimum donation amount must be greater than zero.")]
        public decimal MinAmount { get; set; } = 1.00m;

        [Range(0.01, double.MaxValue, ErrorMessage = "Maximum donation amount must be greater than zero.")]
        public decimal MaxAmount { get; set; } = 10_000.00m;

        [Range(1, 1440, ErrorMessage = "Pending timeout must be between 1 and 1440 minutes.")]
        public int PendingTimeoutMinutes { get; set; } = 30;

        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency must be a 3-letter ISO code.")]
        public string DefaultCurrency { get; set; } = "USD";
    }
}
