

namespace DonationHub.Application.Features.Campaigns.Common
{
    public class CampaignDto
    {
        public Guid Id { get; set; }
        public string TitleAr { get; set; }

        public string TitleEn { get; set; }
        public string DescriptionAr { get; set; }

        public string DescriptionEn { get; set; }

        public decimal GoalAmount { get; set; }

        public DateTimeOffset EndDate { get; set; }


        public Guid CategoryId { get; set; }

        // TODO: TO HANDLE JOIN IN FUTURE
        //public string CategoryName { get; set; }

        //public string CreatedByUserName { get; set; }

    }
}
