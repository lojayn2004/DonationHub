using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Features.Categories.Common;
using DonationHub.Domain.Models.Categories;

namespace DonationHub.Application.Features.Categories.Specifications
{
    internal sealed class CategoryNameSpecification : Specification<Category, CategoryResponse>
    {
        public CategoryNameSpecification(string nameAr, string nameEn)
            : base(category => category.NameAr == nameAr || category.NameEn == nameEn)
        {
            Projection = category => new CategoryResponse()
            {
                Id = category.Id,
                Name = null,
                Description = null,
                NameAr = category.NameAr,
                NameEn = category.NameEn,
                DescriptionAr = category.DescriptionAr,
                DescriptionEn = category.DescriptionEn

            };
        }
    }
}
