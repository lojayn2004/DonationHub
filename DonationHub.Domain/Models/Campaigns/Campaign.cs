using DonationHub.Domain.Exceptions;
using DonationHub.Domain.Models.Categories;
using DonationHub.Domain.Models.Donations;
using DonationHub.Domain.Models.Shared;
using DonationHub.Domain.Results;
using DonationHub.Domain.Results.Errors;

namespace DonationHub.Domain.Models.Campaigns
{
    public class Campaign: BaseEntity, ISoftDeletable, IConcurrency
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

        public decimal ProgressPercent =>
            decimal.Min(100, (RaisedAmount.Amount/ GoalAmount.Amount) * 100);

        public int DaysLeft { get; private set; }


        public int SetDaysLeft(DateTimeOffset utcNow)
        {
            DaysLeft = Math.Max(0, (EndDate - utcNow).Days);
            return DaysLeft;
        }

        public static Result<Campaign> Create(string titleAr, string titleEn,
                                              string descAr, string descEn,
                                              Money goalAmount,
                                              DateTimeOffset endDate,
                                              Guid categoryId,
                                              Guid createdByUserId,
                                              DateTimeOffset utcNow) {
            // TODO: enhance validation messge 
            if (goalAmount.Amount <= 0)
                return Result<Campaign>.Failure(ValidationErrors.Falied);
            if (endDate <= utcNow)
                return Result<Campaign>.Failure(CampaignErrors.EndDateInPast);


            var campaign =  new Campaign
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

            return Result<Campaign>.Success(campaign);
        }

        public void UpdateGoalAmount(Money newGoal, DateTimeOffset utcNow)
        {
            if (newGoal.Amount < 0)
                throw new AmountNotValidException("Goal Amount Must be Positive");
            if (newGoal.Amount < RaisedAmount.Amount)
                throw new AmountNotValidException("Updated Goal Amount Must be Greater than or equal the raised amount");
            // TODO: add when they are not the same currecny also
            GoalAmount = newGoal;
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

        public Result UpdateCampaignStatus(CampaignStatus status, DateTimeOffset utcNow)
        {
            if (CampaignStatus != CampaignStatus.ACTIVE)
                return Result.Failure(CampaignErrors.NotActive);
            CampaignStatus = status;
            UpdatedAt = utcNow;
            return Result.Success();
        }

        public Result Update(string titleAr, string titleEn,
                                              string descAr, string descEn,
                                              decimal goalAmount,
                                              DateTimeOffset endDate,
                                              Guid categoryId,
                                              DateTimeOffset utcNow)
        {
           
            TitleAr = titleAr;
            TitleEn = titleEn;
            DescriptionAr = descAr;
            DescriptionEn = descEn;
            if (endDate <= utcNow)
                return Result.Failure(CampaignErrors.EndDateInPast);

            var updateGoalAmountResult = UpdateGoalAmount(goalAmount, utcNow);
            if (!updateGoalAmountResult.IsSuccess)
                return updateGoalAmountResult;

            EndDate = endDate;
            CategoryId = categoryId;
            UpdatedAt = utcNow;
            return Result.Success();
        }

        private Result UpdateGoalAmount(decimal goalAmount, DateTimeOffset utcNow)
        {
            if (goalAmount <= 0)
                return Result.Failure(ValidationErrors.Falied);

            if (goalAmount < RaisedAmount.Amount)
                return Result.Failure(CampaignErrors.GoalBelowRaised);

            // if their is no update in the goal amount
            if (goalAmount == GoalAmount.Amount)
                return Result.Success();

            GoalAmount.UpdateAmount(goalAmount);

            if (GoalAmount.Amount == RaisedAmount.Amount)
            {
                var result = UpdateCampaignStatus(CampaignStatus.COMPLETED, utcNow);
                if (!result.IsSuccess)
                    return result;

            }
            return Result.Success();
        }


    }
}
