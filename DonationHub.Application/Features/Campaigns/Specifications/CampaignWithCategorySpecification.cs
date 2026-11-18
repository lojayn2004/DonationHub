using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Features.Campaigns.Common;
using DonationHub.Domain.Models.Campaigns;
using System.Linq.Expressions;


namespace DonationHub.Application.Features.Campaigns.Specifications
{
    internal class CampaignWithCategorySpecification: Specification<Campaign, CampaignDto>
    {
      
        public CampaignWithCategorySpecification(Guid id, TimeProvider provider): base(c => c.Id == id)
        {
        
            Projection = GetProjection(provider);
            AddInclude(c => c.Category);

        }

        private static Expression<Func<Campaign, CampaignDto>> GetProjection(TimeProvider provider)
        {
            return campaign => new CampaignDto()
            {
                Id = campaign.Id,
                TitleAr = campaign.TitleAr,
                TitleEn = campaign.TitleEn,
                DescriptionAr = campaign.DescriptionAr,
                DescriptionEn = campaign.DescriptionEn,
                GoalAmount = campaign.GoalAmount.Amount,
                RaisedAmount = campaign.RaisedAmount.Amount,
                EndDate = campaign.EndDate,
                CategoryId = campaign.CategoryId,
                CategoryName = campaign.Category.NameEn,
                RowVersion = campaign.rowVersion,
                Status = campaign.CampaignStatus.ToString(),
                CreatedByUserId = campaign.CreatedByUserId,
                DaysLeft = campaign.SetDaysLeft(provider.GetUtcNow())

            };
        }
    }
}
