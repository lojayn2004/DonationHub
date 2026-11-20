using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Features.Campaigns.Commands.CreateCampaign;
using DonationHub.Domain.Models.Campaigns;
using DonationHub.Domain.Models.Categories;
using DonationHub.Domain.Results;
using DonationHub.Domain.Results.Errors;
using Microsoft.Extensions.Time.Testing;
using FluentAssertions;
using Moq;


namespace DonationHub.Application.UnitTests.CommandTests
{
    public  class CreateCampaignCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _uow = new();
        private readonly Mock<IGenericRepository<Category>> _categories = new();
        private readonly Mock<IGenericRepository<Campaign>> _campaigns = new();
        private readonly FakeTimeProvider _time =
            new(new DateTimeOffset(2026, 11, 17, 0, 0, 0, TimeSpan.Zero));
        private readonly CreateCampaignCommandHandler _handler;



        public CreateCampaignCommandHandlerTests()
        {
            _uow.Setup(u => u.GetRepository<Category>()).Returns(_categories.Object);
            _uow.Setup(u => u.GetRepository<Campaign>()).Returns(_campaigns.Object);
            _uow.Setup(u => u.SavaChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<int>.Success(1));

            _handler = new CreateCampaignCommandHandler(_uow.Object, _time);
        }



        private CreateCampaignCommand BuildCommand(decimal goal = 1000m, int daysAhead = 30) => new(
                 titleAr: "حملة",
                 titleEn: "Campaign",
                 descAr: "وصف",
                 descEn: "Description",
                 goalAmount: goal,
                 goalCurrency: "EGP",                                   
                 endDate: _time.GetUtcNow().AddDays(daysAhead),        
                 categoryId: Guid.NewGuid(),
                 createdByUserId: Guid.NewGuid());


        [Fact] 
        public async Task Handle_UnknownCategory_ReturnsNotFound_AndNeverSaves()
        {
            // Arrange
            var cmd = BuildCommand();
            _categories.Setup(r => r.GetByIdAsync(cmd.categoryId, It.IsAny<CancellationToken>()))
                       .ReturnsAsync((Category?)null);

            // Act
            var result = await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(CategoryErrors.NotFound);
            _campaigns.Verify(r => r.Add(It.IsAny<Campaign>()), Times.Never);
            _uow.Verify(u => u.SavaChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }



        [Fact] 
        public async Task Handle_ValidCommand_AddsCampaign_AndSavesOnce()
        {
            
            var cmd = BuildCommand();
            _categories.Setup(r => r
                                  .GetByIdAsync(cmd.categoryId, It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(Category.CreateCategory("تعليم", "Education", "تعليم", "description", _time.GetUtcNow()));

            Campaign? added = null;
            _campaigns.Setup(r => r.Add(It.IsAny<Campaign>()))
                      .Callback<Campaign>(c => added = c);

            // Act
            var result = await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _campaigns.Verify(r => r.Add(It.IsAny<Campaign>()), Times.Once);
            _uow.Verify(u => u.SavaChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            added.Should().NotBeNull();
            added!.TitleEn.Should().Be("Campaign");
            result.Value.Id.Should().Be(added.Id);
            result.Value.GoalAmount.Should().Be(1000m);
        }

        [Fact]
        public async Task Handle_DomainRuleFails_ReturnsThatError_AndNeverSaves()
        {
          
            var cmd = BuildCommand(goal: 0m);
            _categories.Setup(r => r
                                .GetByIdAsync(cmd.categoryId, It.IsAny<CancellationToken>()))
                                .ReturnsAsync(Category.CreateCategory("تعليم", "Education", "تعليم", "description", _time.GetUtcNow()));


            // Act
            var result = await _handler.Handle(cmd, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Type.Should().Be(ErrorType.Validation);
            _campaigns.Verify(r => r.Add(It.IsAny<Campaign>()), Times.Never);
            _uow.Verify(u => u.SavaChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    
    
    
    
    
    
    
    }
}
