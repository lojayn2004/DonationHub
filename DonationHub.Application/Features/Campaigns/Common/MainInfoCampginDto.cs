
namespace DonationHub.Application.Features.Campaigns.Common
{
    public class MainInfoCampginDto
    {
        public Guid Id { get; set; }
        public string TitleAr { get; set; }

        public string TitleEn { get; set; }
        public string DescriptionAr { get; set; }

        public string DescriptionEn { get; set; }

        public decimal RaisedAmount { get; set; }

        public decimal GoalAmount { get; set; }

        public DateTimeOffset EndDate { get; set; }
    }
}
