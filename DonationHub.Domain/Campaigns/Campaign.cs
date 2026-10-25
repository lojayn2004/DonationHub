using DonationHub.Domain.Categories;
using DonationHub.Domain.Shared;

namespace DonationHub.Domain.Campaigns
{
    public class Campaign: BaseEntity
    {
        public string TitleAr { get; private set; }

        public string TitleEn { get; private set; }

        public string DescriptionAr { get; private set; }

        public string DescriptionEn { get; private set; }

        public DateTime UpdatedAt { get; private set; }

        public DateTime EndDate { get; private set; }

        public Money GoalAmount { get; private set; }

        public Money RaisedAmount { get; private set; }

        public bool IsDeleted { get; private set; }

        public CampaignStatus CampaignStatus { get; private set; }

        public string? BannerFileName { get; private set; }

        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; }

        public byte[] rowVersion { get; private set; }
        
        public Guid CreatedByUserId { get; private set; }

        public ApplicationUser CreatedBy { get; private set; }


        public static Campaign CreateCampaign(string titleAr, string titleEn,
                                              string descAr, string descEn,
                                              Money goalAmount,
                                              DateTime endDate,
                                              Guid categoryId,
                                              Guid createdByUserId,
                                              DateTime utcNow) {
            // TODO: Enhance exception type
            if (goalAmount.Amount <= 0)
                throw new Exception("Goal Amount Must be Positive");
            if(endDate <= utcNow)
                throw new Exception("End Date Must be in future");


            return new Campaign
            {
                Id = Guid.NewGuid(),
                TitleAr = titleAr,
                TitleEn = titleEn,
                DescriptionAr = descAr,
                DescriptionEn = descEn,
                GoalAmount = goalAmount,
                RaisedAmount = Money.Zero(goalAmount.Currency),
                EndDate = endDate,
                CategoryId = categoryId,
                CreatedByUserId = createdByUserId,
                CampaignStatus = CampaignStatus.ACTIVE,
                CreatedAt = utcNow,
                UpdatedAt = utcNow,
                IsDeleted = false
            };
        }

        public void UpdateGoalAmount(Money newGoal,DateTime utcNow)
        {
            if(newGoal.Amount <= 0)
                throw new Exception("Goal Amount Must be Positive");
            GoalAmount = newGoal;
            UpdatedAt = utcNow;
            
        }

        public void SoftDelete(DateTime utcNow)
        {
            IsDeleted = true;
            UpdatedAt = utcNow;
        }






    }
}
