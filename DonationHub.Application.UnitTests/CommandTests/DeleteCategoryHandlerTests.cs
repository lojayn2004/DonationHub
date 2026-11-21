using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Features.Categories.Commands.DeleteCategory;
using DonationHub.Domain.Models.Campaigns;
using DonationHub.Domain.Models.Categories;
using DonationHub.Domain.Models.Shared;
using DonationHub.Domain.Results;
using FluentAssertions;
using Microsoft.Extensions.Time.Testing;
using Moq;

namespace DonationHub.Application.UnitTests.CommandTests
{
    public class DeleteCategoryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        
        private readonly Mock<IGenericRepository<Category>> _categories =  new();

        private readonly DeleteCategoryCommandHandler _handler;
        private readonly FakeTimeProvider _time =
         new(new DateTimeOffset(2026, 11, 17, 0, 0, 0, TimeSpan.Zero));


        public DeleteCategoryHandlerTests()
        {
            _unitOfWork.Setup(u => u.GetRepository<Category>()).Returns(_categories.Object);
           

            _handler = new(_unitOfWork.Object);

        }

        [Fact]
        public async Task Handle_WhenCategoryHasCampaigns_ReturnsConflict()
        {
            var cmd = new DeleteCategoryCommand(Guid.NewGuid());
            var category = GetCategory();
            category.AddCampaign(GetValidCampaign());

            Specification<Category, Category>? usedSpecification = null;
            _categories.Setup(c => c.GetByIdAsync(
                    cmd.Id,
                    It.IsAny<Specification<Category, Category>>(),
                    It.IsAny<CancellationToken>()))
                .Callback<Guid, Specification<Category, Category>, CancellationToken>(
                    (_, specification, _) => usedSpecification = specification)
                .ReturnsAsync(category);

            var result = await _handler.Handle(cmd, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();
            result.Error.Type.Should().Be(Domain.Results.ErrorType.Conflict);
            usedSpecification.Should().NotBeNull();
            usedSpecification!.Includes.Should().ContainSingle();
            usedSpecification.Includes[0].ToString().Should().Contain(nameof(Category.Campaigns));


        }

        private Category GetCategory()
        {
            string NameAr = "الفئة الأولى";
            string NameEn = "First Category";
            string DescriptionAr = "هذا وصف تجريبي للفئة الأولى";
            string DescriptionEn = "This is a dummy description for the first category";
        
            var category = Category.CreateCategory(
                 nameAr: NameAr,
                 nameEn: NameEn,
                 descriptionAr: DescriptionAr,
                 descriptionEn: DescriptionEn,
                 utcNow: _time.GetUtcNow()
             );
            return category;
        }

        private Campaign GetValidCampaign()
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

            result.IsSuccess.Should().BeTrue("helper must produce a valid campaign");
            return result.Value;
        }

    }
}



