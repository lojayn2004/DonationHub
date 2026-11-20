using DonationHub.Domain.Models.Campaigns;
using DonationHub.Domain.Models.Donations;
using DonationHub.Domain.Models.Shared;
using DonationHub.Domain.Results;
using FluentAssertions;
using Microsoft.Extensions.Time.Testing;

namespace DonationHub.Domain.UnitTests
{
    public class CampaignTests
    {
        protected readonly FakeTimeProvider TimeProvider;

        public CampaignTests()
        {
            TimeProvider = new FakeTimeProvider(
                new DateTimeOffset(2025, 1, 15, 10, 0, 0, TimeSpan.Zero));
        }

       
        private Campaign CreateValidCampaign()
        {
            var now = TimeProvider.GetUtcNow();
            var result = Campaign.Create(
                titleAr: "حملة تجريبية",
                titleEn: "Dummy Campaign",
                descAr: "هذا وصف تجريبي للحملة",
                descEn: "This is a dummy description for the campaign",
                goalAmount: new Money(10000, "USD"),
                endDate: now.AddDays(30),
                categoryId: Guid.NewGuid(),
                createdByUserId: Guid.NewGuid(),
                utcNow: now);

            result.IsSuccess.Should().BeTrue("helper must produce a valid campaign");
            return result.Value;
        }

        [Fact]
        public void CreateCampaign_WhenValidData_ReturnsSuccessResult()
        {
            // Arrange
            var now = TimeProvider.GetUtcNow();
            var endDate = now.AddDays(30);

            // Act
            var campaignResult = Campaign.Create(
                titleAr: "حملة تجريبية",
                titleEn: "Dummy Campaign",
                descAr: "هذا وصف تجريبي للحملة",
                descEn: "This is a dummy description for the campaign",
                goalAmount: new Money(10000, "USD"),
                endDate: endDate,
                categoryId: Guid.NewGuid(),
                createdByUserId: Guid.NewGuid(),
                utcNow: now);

            // Assert
            campaignResult.IsSuccess.Should().BeTrue();
            campaignResult.Value.Should().NotBeNull();

            var campaign = campaignResult.Value!;
            campaign.CreatedAt.Should().Be(now);         
           
            campaign.EndDate.Should().Be(endDate);          
            campaign.CampaignStatus.Should().Be(CampaignStatus.ACTIVE);
        }

        [Fact]
        public void CreateCampaign_WhenGoalAmountNotPositive_ReturnsFaliureResult()
        {
            var now = TimeProvider.GetUtcNow();

            var campaignResult = Campaign.Create(
                titleAr: "حملة تجريبية",
                titleEn: "Dummy Campaign",
                descAr: "هذا وصف تجريبي للحملة",
                descEn: "This is a dummy description for the campaign",
                goalAmount: new Money(-1, "USD"),
                endDate: now.AddDays(30),
                categoryId: Guid.NewGuid(),
                createdByUserId: Guid.NewGuid(),
                utcNow: now);

            campaignResult.IsSuccess.Should().BeFalse();
            campaignResult.Error.Should().NotBeNull();
            campaignResult.Error.Type.Should().Be(ErrorType.Validation);
        }

        [Fact]
        public void CreateCampaign_WhenEndDateInPast_ReturnsFaliureResult()
        {
            // Arrange
            var now = TimeProvider.GetUtcNow();

            // Act
            var campaignResult = Campaign.Create(
                titleAr: "حملة تجريبية",
                titleEn: "Dummy Campaign",
                descAr: "هذا وصف تجريبي للحملة",
                descEn: "This is a dummy description for the campaign",
                goalAmount: new Money(10000, "USD"),
                endDate: now,
                categoryId: Guid.NewGuid(),
                createdByUserId: Guid.NewGuid(),
                utcNow: now.AddDays(30));

            // Assert
            campaignResult.IsSuccess.Should().BeFalse();
            campaignResult.Error.Type.Should().Be(ErrorType.UnprocessableEntity);
        }

        [Fact]
        public void UpdateCampaignStatus_WhenStatusActive_UpdatesSuccessfully()
        {
            // Arrange
            var campaign = CreateValidCampaign();
            campaign.CampaignStatus.Should().Be(CampaignStatus.ACTIVE);
            TimeProvider.Advance(TimeSpan.FromHours(3));
            var updateTime = TimeProvider.GetUtcNow();
            // Act
            var result = campaign.UpdateCampaignStatus(CampaignStatus.EXPIRED, updateTime);

            // Assert
            result.IsSuccess.Should().BeTrue();
            campaign.CampaignStatus.Should().Be(CampaignStatus.EXPIRED);
            campaign.UpdatedAt.Should().Be(updateTime);                  
         
        }

        [Fact]
        public void UpdateCampaignStatus_WhenStatusNotActive_ReturnsFaliure()
        {
            // Arrange
            var campaign = CreateValidCampaign();
          
            campaign.CampaignStatus.Should().Be(CampaignStatus.ACTIVE); 

            campaign.UpdateCampaignStatus(CampaignStatus.EXPIRED, TimeProvider.GetUtcNow());
            campaign.CampaignStatus.Should().Be(CampaignStatus.EXPIRED); 

            var updatedAtBeforeSecondCall = campaign.UpdatedAt;

          
            TimeProvider.Advance(TimeSpan.FromHours(1));

            // Act
            var result = campaign.UpdateCampaignStatus(CampaignStatus.ACTIVE, TimeProvider.GetUtcNow());

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            campaign.UpdatedAt.Should().Be(updatedAtBeforeSecondCall); 
        }


        [Fact]
        public void UpdateCamapign_WhenGoalAmountBelowRaised_ReturnsFaliure()
        {

            // Arrange
            var campaign = CreateValidCampaign();


            // add big donation
            var donation = Donation.Create(campaign.Id, campaign.CategoryId, new Money(50000, "USD"),
               int.MinValue, int.MaxValue, TimeProvider.GetUtcNow());
           
            donation.UpdateStatus(DonationStatus.COMPLETED);
            campaign.AddDonation(donation, TimeProvider.GetUtcNow());
            // Act
            var result = campaign.Update(
                   titleAr: "عنوان تجريبي",
                   titleEn: "Dummy Title",
                   descAr: "وصف تجريبي",
                   descEn: "Dummy description",
                   goalAmount: 10_000m,
                   endDate: TimeProvider.GetUtcNow().AddDays(30),
                   categoryId: Guid.NewGuid(),
                   utcNow: TimeProvider.GetUtcNow());

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Type.Should().Be(ErrorType.UnprocessableEntity);
            result.Error.Code.Should().Be("Campaign.GoalBelowRaised");
         

        }



        [Fact]
        public void AddDonation_WhenReachesRaisedGoal_ReturnsSuccess_UpdateStatusToCompleted()
        {
            // Arrange
            var campaign = CreateValidCampaign();

           
            var donation = Donation.Create(campaign.Id, campaign.CategoryId, new Money(10000, "USD"),
                int.MinValue, int.MaxValue, TimeProvider.GetUtcNow());

            donation.UpdateStatus(DonationStatus.COMPLETED);

            // Act
            campaign.AddDonation(donation, TimeProvider.GetUtcNow());

            // Assert 

            campaign.CampaignStatus.Should().Be(CampaignStatus.COMPLETED);



        }

        [Fact]
        public void ProgressPercent_WhenRaisedAmountExceedsGoal_IsCappedAt100()
        {
            // Arrange
            var campaign = CreateValidCampaign();
            var donation = Donation.Create(
                campaign.Id,
                campaign.CategoryId,
                new Money(15_000, "USD"),
                int.MinValue,
                int.MaxValue,
                TimeProvider.GetUtcNow());
            donation.UpdateStatus(DonationStatus.COMPLETED);
            campaign.AddDonation(donation, TimeProvider.GetUtcNow());

            // Act
            var progressPercent = campaign.ProgressPercent;

            // Assert
            progressPercent.Should().Be(100);
        }

    }
}