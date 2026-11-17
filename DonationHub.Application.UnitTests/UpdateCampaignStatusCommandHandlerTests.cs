

using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Features.Campaigns.Commands.CreateCampaign;
using DonationHub.Application.Features.Campaigns.Commands.UpdateCampaignStatus;
using DonationHub.Domain.Models.Campaigns;
using DonationHub.Domain.Models.Categories;
using DonationHub.Domain.Models.Shared;
using DonationHub.Domain.Results;
using DonationHub.Domain.Results.Errors;
using FluentAssertions;
using Microsoft.Extensions.Time.Testing;
using Microsoft.VisualBasic;
using Moq;
using System.Reflection.Metadata;
using System.Text;

namespace DonationHub.Application.UnitTests
{
    public class UpdateCampaignStatusCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _uow = new();
        private readonly Mock<IGenericRepository<Category>> _categories = new();
        private readonly Mock<IGenericRepository<Campaign>> _campaigns = new();
        private readonly FakeTimeProvider _time =
            new(new DateTimeOffset(2026, 11, 17, 0, 0, 0, TimeSpan.Zero));

        private readonly UpdateCampaignStatusCommandHandler _handler;

        private readonly Mock<IConcurrencyHandler<Campaign>> _concurrencyHandler = new();

        public UpdateCampaignStatusCommandHandlerTests()
        {
            _uow.Setup(u => u.GetRepository<Category>()).Returns(_categories.Object);
            _uow.Setup(u => u.GetRepository<Campaign>()).Returns(_campaigns.Object);
            _uow.Setup(u => u.SavaChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<int>.Success(1));


            _handler = new UpdateCampaignStatusCommandHandler(_uow.Object, _time, _concurrencyHandler.Object);
        }
        private UpdateCampaignStatusCommand BuildCommand(byte[] rowVersion) => new UpdateCampaignStatusCommand(Guid.NewGuid(), "EXPIRED", rowVersion);


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


        [Fact]
        public async Task Handle_SaveReturnsConflict_MapsToConcurrencyConflict()
        {
            byte[] rowVersion = Encoding.UTF8.GetBytes("0x000000000000C355");
            var cmd = BuildCommand(rowVersion);
            _campaigns.Setup(r => r.GetByIdAsync(cmd.campaignId, It.IsAny<CancellationToken>()))
                      .ReturnsAsync(CreateValidCampaign());
            _uow.Setup(u => u.SavaChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<int>.Failure(CommonErrors.ConcurrencyConflict));

            var result = await _handler.Handle(cmd, CancellationToken.None);

            result.Error.Should().Be(CampaignErrors.ConcurrencyConflict);
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


//public async Task<Result> Handle(UpdateCampaignStatusCommand request, CancellationToken cancellationToken)
//{
//    var campaignsRepo = _unitOfWork.GetRepository<Campaign>();
//    var campaign = await campaignsRepo.GetByIdAsync(request.campaignId, cancellationToken);
//    if (campaign == null)
//        return Result.Failure(CampaignErrors.NotFound);
//    var status = Enum.Parse<CampaignStatus>(request.status);


//    var updateStatusResult = campaign.UpdateCampaignStatus(status, _timeProvider.GetLocalNow());

//    if (!updateStatusResult.IsSuccess)
//        return Result.Failure(updateStatusResult.Error);
//    _concurrencyHandler.HandleConcurrentUpdate(campaign, request.rowVersion);




//    var result = await _unitOfWork.SavaChangesAsync(cancellationToken);
//    if (!result.IsSuccess)
//    {
//        if (result.Error.Type == ErrorType.Conflict)
//            return Result.Failure(CampaignErrors.ConcurrencyConflict);
//        return Result.Failure(result.Error);
//    }


//    return Result.Success();
//}
//    }