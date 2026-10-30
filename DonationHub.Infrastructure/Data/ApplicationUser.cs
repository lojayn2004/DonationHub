using DonationHub.Domain.Models.Campaigns;
using DonationHub.Domain.Models.Donations;
using Microsoft.AspNetCore.Identity;


namespace DonationHub.Domain.Models.User
{

    //  PreferredLanguage (en | ar، الافتراضي en) --> do we make it enum ?? 

    public class ApplicationUser: IdentityUser<Guid>
    {

        public string FullName { get; private set; }

        public bool IsActive { get; private set; }

        public string PreferredLanguage { get; private set; }


        public List<Donation> Donations { get; private set; }

        public List<Campaign> Campaigns { get; private set; }

        // role: : Admin, CampaignManager, Donor.

    }
}
