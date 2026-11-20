using DonationHub.Application.Common.Contracts;
using DonationHub.Domain.Models.Categories;

namespace DonationHub.Application.Features.Categories.Specifications
{
    internal sealed class CategoryWithCampaignsSpecification : Specification<Category, Category>
    {
        public CategoryWithCampaignsSpecification()
            : base(null)
        {
            Projection = category => category;
            AddInclude(category => category.Campaigns);
        }
    }
}
