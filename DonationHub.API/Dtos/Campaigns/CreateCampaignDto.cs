namespace DonationHub.API.Dtos.Campaigns
{
    public class CreateCampaignDto
    {
        public string TitleAr { get; set; }

        public string TitleEn { get; set; }
        public string DescriptionAr { get; set; }

        public string DescriptionEn { get; set; }

        public decimal GoalAmount { get; set; }

        public  DateTimeOffset EndDate { get; set; }


        public Guid CategoryId { get; set; }

    }
}



