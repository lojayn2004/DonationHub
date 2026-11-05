namespace DonationHub.API.Dtos.Auth
{
    public class CreateStaffUserDto
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        public string Role { get; set; }

        public string? PreferredLanguage { get; set; } = "en";


    }
}
