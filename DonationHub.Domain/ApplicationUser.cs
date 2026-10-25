using DonationHub.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain
{

    //  PreferredLanguage (en | ar، الافتراضي en) --> do we make it enum ?? 

    public class ApplicationUser: BaseEntity
    {
        public string Email { get; private set; }

        public string UserName { get; private set; }

        public string FullName { get; private set; }

        public bool IsActive { get; private set; }

        // role: : Admin, CampaignManager, Donor.

    }
}
