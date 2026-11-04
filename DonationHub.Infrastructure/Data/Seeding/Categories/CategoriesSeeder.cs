using DonationHub.Application.Common.Contracts;
using DonationHub.Domain.Models.Categories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DonationHub.Infrastructure.Data.Seeding.Categories
{
    internal class CategoryDto
    {
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
    }
    internal class CategoriesSeeder(ApplicationDbContext _dbContext): IDataSeeder
    {

        public async Task SeedAllAsync()
        {
         
            if(!(await _dbContext.Category.AnyAsync()))
            {
                var path = Path.Combine(AppContext.BaseDirectory, "Data", "Seeding", "Categories", "categories.json");
                var categoriesJson = await File.ReadAllTextAsync(path);
                var categories = JsonSerializer.Deserialize<IReadOnlyList<CategoryDto>>(categoriesJson);

                
                foreach(var category in categories)
                {
                    await _dbContext.Category.AddAsync(Category.CreateCategory(category.NameAr,
                        category.NameEn, category.DescriptionAr, category.DescriptionEn, DateTime.UtcNow));
                }
                await _dbContext.SaveChangesAsync();
            }
             
        }
    }
}
