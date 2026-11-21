using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Features.Categories.Commands.CreateCategory;
using DonationHub.Application.Features.Categories.Common;
using DonationHub.Domain.Models.Categories;
using DonationHub.Domain.Results;
using DonationHub.Domain.Results.Errors;
using FluentAssertions;
using Microsoft.Extensions.Time.Testing;
using Moq;

namespace DonationHub.Application.UnitTests.CommandTests
{
    public class CreateCategoryCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly Mock<IGenericRepository<Category>> _categories = new();
        private readonly Mock<IQuery<Category, CategoryResponse>> _categoryQuery = new();
        private readonly FakeTimeProvider _time =
            new(new DateTimeOffset(2026, 11, 17, 0, 0, 0, TimeSpan.Zero));
        private readonly CreateCategoryCommandHandler _handler;

        public CreateCategoryCommandHandlerTests()
        {
            _unitOfWork.Setup(unitOfWork => unitOfWork.GetRepository<Category>())
                .Returns(_categories.Object);
            _unitOfWork.Setup(unitOfWork => unitOfWork.SavaChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<int>.Success(1));

            _categoryQuery
                .Setup(query => query.GetAllAsync(It.IsAny<Specification<Category, CategoryResponse>>(), It.IsAny<CancellationToken>()))
                .Returns((Specification<Category, CategoryResponse> specification) =>
                {
                    var existingCategories = new[]
                    {
                        Category.CreateCategory(
                            "الفئة الموجودة",
                            "Existing Category",
                            "وصف",
                            "Description",
                            _time.GetUtcNow())
                    };
                    var matches = existingCategories
                        .Where(category => specification.Criteria!.Compile()(category))
                        .Select(category => new CategoryResponse(
                            category.Id,
                            category.NameAr,
                            category.NameEn,
                            category.DescriptionAr,
                            category.DescriptionEn))
                        .ToList();

                    return Task.FromResult<IReadOnlyList<CategoryResponse>>(matches);
                });

            _handler = new CreateCategoryCommandHandler(
                _unitOfWork.Object,
                _categoryQuery.Object,
                _time);
        }

        [Theory]
        [InlineData("الفئة الموجودة", "Different English Name")]
        [InlineData("اسم مختلف", "Existing Category")]
        public async Task Handle_WhenEitherNameAlreadyExists_ReturnsConflictWithoutSaving(
            string nameAr,
            string nameEn)
        {
            var command = new CreateCategoryCommand(nameAr, nameEn, "وصف", "Description");

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(CategoryErrors.AlreadyExists);
            _categories.Verify(repository => repository.Add(It.IsAny<Category>()), Times.Never);
            _unitOfWork.Verify(
                unitOfWork => unitOfWork.SavaChangesAsync(It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_WhenNeitherNameExists_CreatesCategory()
        {
            var command = new CreateCategoryCommand("اسم جديد", "New Category", "وصف", "Description");

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            _categories.Verify(repository => repository.Add(It.IsAny<Category>()), Times.Once);
            _unitOfWork.Verify(
                unitOfWork => unitOfWork.SavaChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
