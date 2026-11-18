using System.ComponentModel.DataAnnotations;

namespace DonationHub.API.Dtos.Auth
{
    public class RegisterDto
    {
        public string  Email { get; set; } = default!;

        public string Password { get; set; } = default!;

        public string FullName { get; set; } = default!;

        public string UserName { get; set; } = default!;

        [RegularExpression("^(en|ar)$", ErrorMessage = "Preferred language must be 'en' or 'ar'.")]
        public string? PreferredLanguage { get; set; } = "en";

    }
}
