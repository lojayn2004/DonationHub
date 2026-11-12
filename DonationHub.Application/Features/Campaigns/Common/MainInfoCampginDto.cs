
namespace DonationHub.Application.Features.Campaigns.Common
{
    public class MainInfoCampginDto
    {
        public Guid Id { get; set; }
        public string TitleAr { get; set; } = default!;

        public string TitleEn { get; set; } = default!;
        public string DescriptionAr { get; set; } = default!;

        public string DescriptionEn { get; set; } = default!;

        public decimal RaisedAmount { get; set; } = default!;

        public decimal GoalAmount { get; set; } = default!;

        public DateTimeOffset EndDate { get; set; }
    }
}
