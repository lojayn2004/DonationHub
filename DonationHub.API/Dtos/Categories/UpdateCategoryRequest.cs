namespace DonationHub.API.Dtos.Categories
{
    public class UpdateCategoryRequest
    {
        public string NameAr { get; set; } = default!;

        public string NameEn { get; set; } = default!;

        public string? DescriptionAr { get; set; }

        public string? DescriptionEn { get; set; }
    }
}
