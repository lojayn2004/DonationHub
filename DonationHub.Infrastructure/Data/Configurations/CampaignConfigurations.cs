using DonationHub.Domain.Models.Campaigns;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DonationHub.Infrastructure.Data.Configurations
{
    internal class CampaignConfigurations : IEntityTypeConfiguration<Campaign>
    {
        public void Configure(EntityTypeBuilder<Campaign> builder)
        {
            builder.ToTable("Campaigns");

            builder.HasKey(c => c.Id);

         
            builder.Property(c => c.TitleAr)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.TitleEn)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.DescriptionAr)
                .IsRequired()
                .HasMaxLength(4000);

            builder.Property(c => c.DescriptionEn)
                .IsRequired()
                .HasMaxLength(4000);

            builder.Property(c => c.UpdatedAt)
                .IsRequired();

            builder.Property(c => c.EndDate)
                .IsRequired();

            builder.Property(c => c.CampaignStatus)
                .IsRequired()
                .HasConversion<string>()   
                .HasMaxLength(50);

            builder.Property(c => c.BannerFileName)
                .HasMaxLength(500);

            builder.Property(c => c.CreatedByUserId)
                .IsRequired();

         
            builder.Property(c => c.rowVersion)
                .IsRowVersion();



            //  Relations

            builder.OwnsOne(c => c.GoalAmount, money =>
            {
                money.Property(m => m.Amount)
                    .HasColumnName("GoalAmount_Amount")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                money.Property(m => m.Currency)
                    .HasColumnName("GoalAmount_Currency")
                    .HasMaxLength(3)
                    .IsRequired();
            });

            builder.OwnsOne(c => c.RaisedAmount, money =>
            {
                money.Property(m => m.Amount)
                    .HasColumnName("RaisedAmount_Amount")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                money.Property(m => m.Currency)
                    .HasColumnName("RaisedAmount_Currency")
                    .HasMaxLength(3)
                    .IsRequired();

            });

            builder.HasMany(c => c.Donations)
                   .WithOne(d => d.Campaign)
                   .HasForeignKey(d => d.CampaignId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
