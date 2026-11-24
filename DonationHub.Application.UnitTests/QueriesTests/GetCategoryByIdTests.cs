using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Features.Categories.Common;
using DonationHub.Application.Features.Categories.Queries.GetCategoryById;
using DonationHub.Domain.Models.Categories;
using FluentAssertions;
using Moq;

namespace DonationHub.Application.UnitTests.QueriesTests;

public class GetCategoryByIdTests
{
    private static readonly Guid CategoryId = Guid.Parse("a73620d8-a359-4c5c-90e9-9058b6b332f7");

    private readonly Mock<IQuery<Category, Category>> _query = new();
    private readonly Mock<ICurrentLanguage> _language = new();
    private readonly GetCategoryByIdQueryHandler _handler;

    public GetCategoryByIdTests()
    {
        _handler = new GetCategoryByIdQueryHandler(_query.Object, _language.Object);
    }

    [Fact]
    public async Task Handle_WhenAdminRequestsCategory_ReturnsBothLanguages()
    {
        SetupCategory(isArabic: false);

        var result = await _handler.Handle(
            new GetCategoryByIdQuery(CategoryId, isAdmin: true),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Name.Should().BeNull();
        result.Value.Description.Should().BeNull();
        result.Value.NameAr.Should().Be("الفئة الأولى");
        result.Value.NameEn.Should().Be("First category");
        result.Value.DescriptionAr.Should().Be("وصف الفئة");
        result.Value.DescriptionEn.Should().Be("Category description");
    }

    [Theory]
    [InlineData(true, "الفئة الأولى", "وصف الفئة")]
    [InlineData(false, "First category", "Category description")]
    public async Task Handle_WhenUserRequestsCategory_ReturnsOnlyRequestedLanguage(
        bool isArabic,
        string expectedName,
        string expectedDescription)
    {
        SetupCategory(isArabic);

        var result = await _handler.Handle(
            new GetCategoryByIdQuery(CategoryId, isAdmin: false),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Name.Should().Be(expectedName);
        result.Value.Description.Should().Be(expectedDescription);
        result.Value.NameAr.Should().BeNull();
        result.Value.NameEn.Should().BeNull();
        result.Value.DescriptionAr.Should().BeNull();
        result.Value.DescriptionEn.Should().BeNull();
    }

    private void SetupCategory(bool isArabic)
    {
        var category = Category.CreateCategory(
            "الفئة الأولى",
            "First category",
            "وصف الفئة",
            "Category description",
            DateTimeOffset.UtcNow);

        _query.Setup(query => query.GetByIdAsync(
                CategoryId,
                It.IsAny<Specification<Category, Category>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);
        _language.Setup(currentLanguage => currentLanguage.IsArabic).Returns(isArabic);
    }
}