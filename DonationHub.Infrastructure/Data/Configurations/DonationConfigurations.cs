using DonationHub.Domain.Models.Donations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DonationHub.Infrastructure.Data.Configurations
{
    internal class DonationConfigurations : IEntityTypeConfiguration<Donation>
    {
        public void Configure(EntityTypeBuilder<Donation> builder)
        {
            builder.HasKey(d => d.Id);


            builder.Property(d => d.FaliureReson)
                   .HasMaxLength(500);

            builder.Property(d => d.ReceiptNumber)
                   .HasMaxLength(500);


            builder.Property(d => d.ReceiptFileName)
                   .HasMaxLength(1000);

            builder.Property(d => d.DonationStatus)
                   .HasConversion<string>()
                   .HasMaxLength(50);

            builder.OwnsOne(c => c.Amount, money =>
            {
                money.Property(m => m.Amount)
                       .HasColumnName("DonationAmount")
                       .HasColumnType("decimal(18,2)")
                       .IsRequired();

                money.Property(m => m.Currency)
                      .HasColumnName("DonationCurrency")
                      .HasMaxLength(3)
                      .IsRequired();

            });

        }
    }
}
