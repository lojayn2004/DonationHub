using System.ComponentModel.DataAnnotations;

namespace DonationHub.API.Dtos.Auth
{
    public class RegisterDto
    {
        public string  Email { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        [RegularExpression("^(en|ar)$", ErrorMessage = "Preferred language must be 'en' or 'ar'.")]
        public string? PreferredLanguage { get; set; } = "en";

    }
}
