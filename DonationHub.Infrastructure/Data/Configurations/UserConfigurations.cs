using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DonationHub.Infrastructure.Data.Configurations
{
    internal class UserConfigurations : IEntityTypeConfiguration<ApplicationUser>
    {

        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasMany(u => u.Donations)
                .WithOne()
                .HasForeignKey(d => d.DonorUserId);

            builder.HasMany(u => u.Campaigns)
                .WithOne()
                .HasForeignKey(d => d.CreatedByUserId);

            builder.Property(u => u.FullName)
                    .HasMaxLength(250);

            builder.Property(u => u.PreferredLanguage)
                    .HasMaxLength(3);

            builder.Property(c => c.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Idempotency record Relation 

        }
    }
}
