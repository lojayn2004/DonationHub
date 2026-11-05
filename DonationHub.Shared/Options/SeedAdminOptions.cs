using System.ComponentModel.DataAnnotations;

namespace DonationHub.Shared.Options
{
    public class SeedAdminOptions
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FullName { get; set; }



    }
}
