using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Features.Campaigns.Commands.UpdateCampaign;
using DonationHub.Domain.Models.Campaigns;
using DonationHub.Domain.Models.Categories;
using DonationHub.Domain.Models.Shared;
using DonationHub.Domain.Results;
using DonationHub.Domain.Results.Errors;
using FluentAssertions;
using Microsoft.Extensions.Time.Testing;
using Moq;
using System.Text;

namespace DonationHub.Application.UnitTests.CommandTests
{
    public class UpdateCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _uow = new();
        private readonly Mock<IGenericRepository<Category>> _categories = new();
        private readonly Mock<IGenericRepository<Campaign>> _campaigns = new();
        private readonly FakeTimeProvider _time =
            new(new DateTimeOffset(2026, 11, 17, 0, 0, 0, TimeSpan.Zero));

        private readonly UpdateCampaignHandler _handler;

        private readonly Mock<IConcurrencyHandler<Campaign>> _concurrencyHandler = new();


        public UpdateCommandHandlerTests()
        {
            _uow.Setup(u => u.GetRepository<Category>()).Returns(_categories.Object);
            _uow.Setup(u => u.GetRepository<Campaign>()).Returns(_campaigns.Object);
            _uow.Setup(u => u.SavaChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<int>.Success(1));


            _handler = new UpdateCampaignHandler(_uow.Object, _time, _concurrencyHandler.Object);
        }
        private UpdateCampaignCommand BuildCommand(byte[] rowVersion) =>
            new UpdateCampaignCommand(Guid.NewGuid(), "حملة تجريب", "english title", "حملة تجريب", "english desc", 1000000,
                _time.GetUtcNow().AddDays(30), Guid.NewGuid(), rowVersion);

        [Fact] 
        public async Task Handler_RowVersionNotValid_MapToCuncurrencyConflict()
        {
            byte[] rowVersion = Encoding.UTF8.GetBytes("0x000000000000C355");
            var cmd = BuildCommand(rowVersion);

            _campaigns.Setup(u => u.GetByIdAsync(cmd.campaignId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(CreateValidCampaign());
                
            _uow.Setup(u => u.SavaChangesAsync(It.IsAny<CancellationToken>()))
               .ReturnsAsync(Result<int>.Failure(CommonErrors.ConcurrencyConflict));

            var result = await _handler.Handle(cmd, CancellationToken.None);

            result.Error.Should().Be(CampaignErrors.ConcurrencyConflict);
        }

        [Fact]
        public async Task Handle_UnknownCampaign_ReturnsNotFound_NeverSaves()
        {
            byte[] rowVersion = Encoding.UTF8.GetBytes("0x000000000000C355");
            var cmd = BuildCommand(rowVersion);
            _campaigns.Setup(r => r.GetByIdAsync(cmd.campaignId, It.IsAny<CancellationToken>()))
                      .ReturnsAsync((Campaign?)null);

            var result = await _handler.Handle(cmd, It.IsAny<CancellationToken>());

            result.Error.Should().Be(CampaignErrors.NotFound);
            _uow.Verify(u => u.SavaChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }




        private Campaign CreateValidCampaign()
        {
            var now = _time.GetUtcNow();
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


            return result.Value;
        }



    }
}
