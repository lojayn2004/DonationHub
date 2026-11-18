namespace DonationHub.API.Dtos.Auth
{
    public class CreateStaffUserDto
    {
        public string Email { get; set; } = default!;

        public string Password { get; set; } = default!;

        public string FullName { get; set; } = default!;

        public string UserName { get; set; } = default!;

        public string Role { get; set; } = default!;

        public string? PreferredLanguage { get; set; } = "en";


    }
}
