
using System.Text.Json.Serialization;

namespace DonationHub.Application.Features.Campaigns.Common
{
    public class MainInfoCampginDto
    {
        public Guid Id { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Title { get; set; } = default!;


        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Description { get; set; } = default!;


        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TitleAr { get; set; } = default!;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TitleEn { get; set; } = default!;


        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? DescriptionAr { get; set; } = default!;


        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? DescriptionEn { get; set; } = default!;

        public decimal RaisedAmount { get; set; } = default!;

        public decimal GoalAmount { get; set; } = default!;

        public DateTimeOffset EndDate { get; set; }

        public decimal ProgressPercent { get; set; }

        public int DaysLeft { get; set; }
    }
}
