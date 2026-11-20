using DonationHub.Domain.Models.Categories;
using FluentAssertions;
using Microsoft.Extensions.Time.Testing;

namespace DonationHub.Domain.UnitTests
{
    public class CategoryTests
    {
        private readonly FakeTimeProvider _time =
          new(new DateTimeOffset(2026, 11, 17, 0, 0, 0, TimeSpan.Zero));


        [Fact]
        public void CreateCategory_ShouldReturnCategoryObjectWithCorrectValues()
        {
            string NameAr = "الفئة الأولى";
            string NameEn = "First Category";
            string DescriptionAr = "هذا وصف تجريبي للفئة الأولى";
            string DescriptionEn = "This is a dummy description for the first category";
            DateTimeOffset UtcNow = _time.GetUtcNow();


            var category = Category.CreateCategory(
                 nameAr: NameAr,
                 nameEn: NameEn,
                 descriptionAr: DescriptionAr,
                 descriptionEn: DescriptionEn,
                 utcNow: UtcNow
             );

            category.NameAr.Should().Be(NameAr);
            category.NameEn.Should().Be(NameEn);
            category.DescriptionAr.Should().Be(DescriptionAr);
            category.DescriptionEn.Should().Be(DescriptionEn);
            category.CreatedAt.Should().Be(UtcNow);


        }
    }
}
