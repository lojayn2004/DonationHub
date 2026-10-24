using DonationHub.Domain.Shared;

namespace DonationHub.Domain.Campaigns
{
    public class Campaign: BaseEntity
    {
        public string TitleAr { get; private set; }

        public string TitleEn { get; private set; }

        public string DescriptionAr { get; private set; }

        public string DescriptionEn { get; private set; }

        public DateTime UpdateAt { get; private set; }

        public DateTime EndDate { get; private set; }

        public Money GoalAmount { get; private set; }

        public Money RaisedAmount { get; private set; }

        public bool IsDeleted { get; private set; }

        public CampaignStatus CampaignStatus { get; private set; }



    }
}
