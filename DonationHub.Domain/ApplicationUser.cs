
using Microsoft.AspNetCore.Identity;


namespace DonationHub.Domain
{

    //  PreferredLanguage (en | ar، الافتراضي en) --> do we make it enum ?? 

    public class ApplicationUser: IdentityUser<Guid>
    {
       

        public string FullName { get; private set; }

        public bool IsActive { get; private set; }

        public string PreferredLanguage { get; private set; }

        // role: : Admin, CampaignManager, Donor.

    }
}
