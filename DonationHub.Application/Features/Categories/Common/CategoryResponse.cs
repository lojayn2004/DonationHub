
using System.Text.Json.Serialization;

namespace DonationHub.Application.Features.Categories.Common
{
    public class CategoryResponse
    {
        public Guid Id { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; } = default!;


        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Description { get; set; } = default!;


        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? NameAr { get; set; } = default!;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? NameEn { get; set; } = default!;


        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? DescriptionAr { get; set; } = default!;


        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? DescriptionEn { get; set; } = default!;
    }
}
