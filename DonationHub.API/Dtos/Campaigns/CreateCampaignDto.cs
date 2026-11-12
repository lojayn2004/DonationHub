namespace DonationHub.API.Dtos.Campaigns
{
    public class CreateCampaignDto
    {
        public string TitleAr { get; set; } = default!;

        public string TitleEn { get; set; } = default!;
        public string DescriptionAr { get; set; } = default!;

        public string DescriptionEn { get; set; } = default!;

        public decimal GoalAmount { get; set; }

        public  DateTimeOffset EndDate { get; set; }


        public Guid CategoryId { get; set; }

    }
}



