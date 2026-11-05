using DonationHub.Domain.Models.Campaigns;
using DonationHub.Domain.Models.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DonationHub.Infrastructure.Data.Configurations
{
    internal class CategoryConfigurations : IEntityTypeConfiguration<Category>
    {
      
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.NameAr)
                   .HasMaxLength(200);

            builder.Property(c => c.NameEn)
                   .HasMaxLength(200);

            builder.Property(c => c.DescriptionAr)
                 .HasMaxLength(4000);

            builder.Property(c => c.DescriptionEn)
                 .HasMaxLength(4000);

            builder.HasMany(c => c.Campaigns)
                   .WithOne(c => c.Category)
                   .HasForeignKey(c => c.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
