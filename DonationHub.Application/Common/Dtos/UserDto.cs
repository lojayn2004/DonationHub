namespace DonationHub.Application.Common.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string PreferredLanguage { get; set; }
    }
}
