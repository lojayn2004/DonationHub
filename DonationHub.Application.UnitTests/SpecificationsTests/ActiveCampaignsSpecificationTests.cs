using DonationHub.Application.Features.Campaigns.Specifications;
using DonationHub.Domain.Models.Campaigns;
using DonationHub.Domain.Models.Shared;
using FluentAssertions;
using Microsoft.Extensions.Time.Testing;

namespace DonationHub.Application.UnitTests.Campaigns
{
    public class ActiveCampaignsSpecificationTests
    {
        [Fact]
        public void Projection_DaysLeftUsesTimeProvider_NeverReturnsNegative()
        {
            // Arrange
            var timeProvider = new FakeTimeProvider(
                new DateTimeOffset(2025, 1, 15, 10, 0, 0, TimeSpan.Zero));
            var now = timeProvider.GetUtcNow();
            var campaignResult = Campaign.Create(
                titleAr: "حملة تجريبية",
                titleEn: "Dummy Campaign",
                descAr: "هذا وصف تجريبي للحملة",
                descEn: "This is a dummy description for the campaign",
                goalAmount: new Money(10_000, "USD"),
                endDate: now.AddDays(10),
                categoryId: Guid.NewGuid(),
                createdByUserId: Guid.NewGuid(),
                utcNow: now);
            campaignResult.IsSuccess.Should().BeTrue();
            var campaign = campaignResult.Value!;
            var specification = new ActiveCampaignsSpecification(true, timeProvider);
            var projection = specification.Projection.Compile();

            // Act
            timeProvider.Advance(TimeSpan.FromDays(3));
            var daysLeftBeforeEnd = projection(campaign).DaysLeft;
            timeProvider.Advance(TimeSpan.FromDays(8));
            var daysLeftAfterEnd = projection(campaign).DaysLeft;

            // Assert
            daysLeftBeforeEnd.Should().Be(7);
            daysLeftAfterEnd.Should().Be(0);
        }
    }
}
