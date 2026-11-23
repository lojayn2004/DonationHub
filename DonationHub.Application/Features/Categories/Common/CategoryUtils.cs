using DonationHub.Domain.Models.Categories;


namespace DonationHub.Application.Features.Categories.Common
{
    public static class CategoryUtils
    {
        public static CategoryResponse ToLanguageResult(this Category category,
            bool isAdmin,
            bool isArabic)
        {
            var categoryResponse = new CategoryResponse()
            {
                Id = category.Id,
                NameAr = category.NameAr,
                NameEn = category.NameEn,
                DescriptionAr = category.DescriptionAr,
                DescriptionEn = category.DescriptionEn

            };
            if (isAdmin)
            {
         
                return categoryResponse;
            }
            categoryResponse.Name = isArabic ? category.NameAr : category.NameEn;
            categoryResponse.Description = isArabic ? category.DescriptionAr : category.DescriptionEn;


            categoryResponse.NameAr = null;
            categoryResponse.DescriptionAr = null;
            categoryResponse.NameEn = null;
            categoryResponse.DescriptionEn = null;
            return categoryResponse;
        }
    }
}
