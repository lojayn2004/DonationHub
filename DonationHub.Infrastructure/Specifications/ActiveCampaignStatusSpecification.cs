using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Features.Campaigns.Common;
using DonationHub.Domain.Models.Campaigns;
using System.Linq.Expressions;

namespace DonationHub.Infrastructure.Specifications
{
    public class ActiveCampaignsSpecification: Specification<Campaign, MainInfoCampginDto>
    {
        public ActiveCampaignsSpecification(bool activeStatus)
            : base(ActiveWhereClause(activeStatus))
        {
            Projection = GetProjection();

            
        }
        private static Expression<Func<Campaign, bool>> ActiveWhereClause(bool activeStatus)
        {
            return activeStatus ? (c => c.CampaignStatus == CampaignStatus.ACTIVE) :
                (c => c.CampaignStatus == CampaignStatus.COMPLETED);
        }

        private static Expression<Func<Campaign, MainInfoCampginDto>> GetProjection()
        {
            return campaign => new MainInfoCampginDto()
            {
                TitleAr = campaign.TitleAr,
                TitleEn = campaign.TitleEn,
                DescriptionAr = campaign.DescriptionAr,
                DescriptionEn = campaign.DescriptionEn,
                GoalAmount = campaign.GoalAmount.Amount,
                RaisedAmount = campaign.RaisedAmount.Amount,
                EndDate = campaign.EndDate

            };
        }
    }
    
}