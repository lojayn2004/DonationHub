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
            Projection = category => new CategoryResponse(
                category.Id,
                category.NameAr,
                category.NameEn,
                category.DescriptionAr,
                category.DescriptionEn);
        }
    }
}
