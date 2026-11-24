using DonationHub.Application.Common.Contracts;
using DonationHub.Domain.Models.Categories;

namespace DonationHub.Application.Features.Categories.Specifications
{
    internal sealed class CategorySpecification : Specification<Category, Category>
    {
        public CategorySpecification()
            : base(null)
        {
            Projection = category => category;
        }
    }
}
