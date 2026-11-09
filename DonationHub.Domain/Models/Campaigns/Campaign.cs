using DonationHub.Domain.Exceptions;
using DonationHub.Domain.Models.Categories;
using DonationHub.Domain.Models.Donations;
using DonationHub.Domain.Models.Shared;

namespace DonationHub.Domain.Models.Campaigns
{
    public class Campaign: BaseEntity, ISoftDeletable
    {
        public string TitleAr { get; private set; }

        public string TitleEn { get; private set; }

        public string DescriptionAr { get; private set; }

        public string DescriptionEn { get; private set; }

        public DateTimeOffset UpdatedAt { get; private set; }

        public DateTimeOffset EndDate { get; private set; }

        public Money GoalAmount { get; private set; }

        public Money RaisedAmount { get; private set; }

        public CampaignStatus CampaignStatus { get; private set; }

        public string? BannerFileName { get; private set; }

        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; }

        public byte[] rowVersion { get; private set; }
        
        public Guid CreatedByUserId { get; private set; }

        public List<Donation> Donations { get; private set; }
        public bool IsDeleted { get ; set; }
        public DateTimeOffset? DeletedAtUtc { get; set; }

        public static Campaign Create(string titleAr, string titleEn,
                                              string descAr, string descEn,
                                              Money goalAmount,
                                              DateTimeOffset endDate,
                                              Guid categoryId,
                                              Guid createdByUserId,
                                              DateTimeOffset utcNow) {
            
            if (goalAmount.Amount < 0)
                throw new AmountNotValidException("Goal Amount Must be Positive");
            if(endDate <= utcNow)
                throw new DateNotValidException("End Date Must be in future");


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
                UpdatedAt = utcNow
            };
        }

        public void UpdateGoalAmount(Money newGoal, DateTimeOffset utcNow)
        {
            if(newGoal.Amount < 0)
                throw new AmountNotValidException("Goal Amount Must be Positive");
            if(newGoal.Amount < RaisedAmount.Amount)
                throw new AmountNotValidException("Updated Goal Amount Must be Greater than or equal the raised amount");
            // TODO: add when they are not the same currecny also
            GoalAmount = newGoal;
            UpdatedAt = utcNow;
            
        }

        public void UpdateEndDate(DateTimeOffset endDate, DateTimeOffset utcNow)
        {
            if (endDate < utcNow)
                throw new DateNotValidException("End Date must be in future");
            EndDate = endDate;
            UpdatedAt = utcNow;
        }


       
        public void AddDonation(Donation donation, DateTimeOffset utcNow)
        {
            if (donation.DonationStatus != DonationStatus.COMPLETED)
                throw new Exception("Only succeeded donations count toward raised amount.");
            if (donation.CampaignId != Id)
                throw new Exception("Donation does not belong to this campaign.");

            if (CampaignStatus != CampaignStatus.ACTIVE || utcNow >= EndDate)
                throw new Exception("cannot add donation to un active campaign");

            RaisedAmount += donation.Amount;
           
            if (RaisedAmount.Amount >= GoalAmount.Amount && CampaignStatus == CampaignStatus.ACTIVE)
                UpdateCampaignStatus(CampaignStatus.COMPLETED, utcNow);
        }

        public void UpdateCampaignStatus(CampaignStatus status, DateTimeOffset utcNow)
        {
            if (CampaignStatus != CampaignStatus.ACTIVE)
                throw new Exception("Cannot update campiagn status it is final");
            CampaignStatus = status;
            UpdatedAt = utcNow;
        }


    }
}
