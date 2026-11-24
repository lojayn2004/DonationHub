using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Features.Campaigns.Common;
using DonationHub.Application.Features.Campaigns.Queries.GetCampaignById;
using DonationHub.Domain.Models.Campaigns;
using FluentAssertions;
using Moq;

namespace DonationHub.Application.UnitTests.QueriesTests;

public class GetCampaignByIdQueryTests
{
    private static readonly Guid CampaignId = Guid.Parse("a73620d8-a359-4c5c-90e9-9058b6b332f7");

    private readonly Mock<IQuery<Campaign, CampaignDto>> _query = new();
    private readonly Mock<ICurrentLanguage> _language = new();
    private readonly GetCampaignByIdQueryHandler _handler;

    public GetCampaignByIdQueryTests()
    {
        _handler = new GetCampaignByIdQueryHandler(
            _query.Object,
            _language.Object,
            TimeProvider.System);
    }

    [Fact]
    public async Task Handle_WhenAdminRequestsCampaign_ReturnsBothLanguages()
    {
        SetupCampaign(isArabic: false);

        var result = await _handler.Handle(new GetCampaignById(CampaignId, isAdmin: true), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Title.Should().BeNull();
        result.Value.Description.Should().BeNull();
        result.Value.TitleAr.Should().Be("حملة إغاثة");
        result.Value.TitleEn.Should().Be("Relief campaign");
        result.Value.DescriptionAr.Should().Be("وصف الحملة");
        result.Value.DescriptionEn.Should().Be("Campaign description");
    }

    [Theory]
    [InlineData(true, "حملة إغاثة", "وصف الحملة")]
    [InlineData(false, "Relief campaign", "Campaign description")]
    public async Task Handle_WhenUserRequestsCampaign_ReturnsOnlyRequestedLanguage(
        bool isArabic,
        string expectedTitle,
        string expectedDescription)
    {
        SetupCampaign(isArabic);

        var result = await _handler.Handle(new GetCampaignById(CampaignId, isAdmin: false), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Title.Should().Be(expectedTitle);
        result.Value.Description.Should().Be(expectedDescription);
        result.Value.TitleAr.Should().BeNull();
        result.Value.TitleEn.Should().BeNull();
        result.Value.DescriptionAr.Should().BeNull();
        result.Value.DescriptionEn.Should().BeNull();
    }

    private void SetupCampaign(bool isArabic)
    {
        _language.Setup(currentLanguage => currentLanguage.IsArabic).Returns(isArabic);
        _query.Setup(query => query.GetByIdAsync(
                CampaignId,
                It.IsAny<Specification<Campaign, CampaignDto>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CampaignDto
            {
                Id = CampaignId,
                TitleAr = "حملة إغاثة",
                TitleEn = "Relief campaign",
                DescriptionAr = "وصف الحملة",
                DescriptionEn = "Campaign description",
                CategoryName = "Emergency",
                RowVersion = [],
                Status = "ACTIVE"
            });
    }
}