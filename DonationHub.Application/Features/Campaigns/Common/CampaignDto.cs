

namespace DonationHub.Application.Features.Campaigns.Common
{
    public class CampaignDto()
    {
        public Guid Id { get; set; }
        public string TitleAr { get; set; } = default!;

        public string TitleEn { get; set; } = default!;
        public string DescriptionAr { get; set; } = default!;

        public string DescriptionEn { get; set; } = default!;

        public decimal GoalAmount { get; set; }


        public decimal RaisedAmount { get; set; }


        public DateTimeOffset EndDate { get; set; }

       

        public Guid CategoryId { get; set; }


        public string CategoryName { get; set; } = default!;


        public byte[] RowVersion { get; set; } = default!;

        public string Status { get; set; } = default!;

        public Guid CreatedByUserId { get; set; }

        public decimal ProgressPercent { get; set; }

        public int DaysLeft { get; set; }

    }
}


