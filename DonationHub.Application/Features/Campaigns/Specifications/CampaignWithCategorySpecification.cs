

using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Features.Campaigns.Common;
using DonationHub.Domain.Models.Campaigns;
using System.Linq.Expressions;


namespace DonationHub.Application.Features.Campaigns.Specifications
{
    internal class CampaignWithCategorySpecification: Specification<Campaign, CampaignDto>
    {
        public CampaignWithCategorySpecification(Guid id): base(c => c.Id == id)
        {
            Projection = GetProjection();
            AddInclude(c => c.Category);

        }

        private static Expression<Func<Campaign, CampaignDto>> GetProjection()
        {
            return campaign => new CampaignDto()
            {

                TitleAr = campaign.TitleAr,
                TitleEn = campaign.TitleEn,
                DescriptionAr = campaign.DescriptionAr,
                DescriptionEn = campaign.DescriptionEn,
                GoalAmount = campaign.GoalAmount.Amount,
                RaisedAmount = campaign.RaisedAmount.Amount,
                EndDate = campaign.EndDate,
                CategoryId = campaign.CategoryId,
                CategoryName = campaign.Category.NameEn
                

            };
        }
    }
}
