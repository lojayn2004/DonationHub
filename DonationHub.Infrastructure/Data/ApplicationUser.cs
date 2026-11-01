using DonationHub.Domain.Models.Campaigns;
using DonationHub.Domain.Models.Donations;
using Microsoft.AspNetCore.Identity;


namespace DonationHub.Infrastructure.Data
{
    public class ApplicationUser: IdentityUser<Guid>
    {

        public string FullName { get; private set; }

        public bool IsActive { get; private set; }

        public string PreferredLanguage { get; private set; }


        public List<Donation> Donations { get; private set; }

        public List<Campaign> Campaigns { get; private set; }

        public static ApplicationUser Create(string fullName,
            string email, 
            string userName,
            string? preferredLanguage)
        {
            var user = new ApplicationUser
            {
                FullName = fullName,
                Email = email,
                UserName = userName,
                IsActive = true,
                PreferredLanguage = preferredLanguage ?? "en"
            };
            return user;
        }

        // role: : Admin, CampaignManager, Donor.

    }
}
