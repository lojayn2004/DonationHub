using DonationHub.Domain.Models.Payments;
using DonationHub.Domain.Models.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DonationHub.Infrastructure.Data.Configurations
{
    internal class PaymentConfigurations : IEntityTypeConfiguration<PaymentAttempt>
    {
        public void Configure(EntityTypeBuilder<PaymentAttempt> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.FaliureCode)
                   .HasMaxLength(10);

            builder.Property(d => d.PaymentAttemptStatus)
                   .HasConversion<string>()
                   .HasMaxLength(50);


            builder.Property(d => d.StripeCheckoutSessionId)
                   .HasMaxLength(600);

            builder.Property(d => d.StripePaymentIntentId)
                   .HasMaxLength(600);

            builder.HasOne(d => d.Donation)
                   .WithOne()
                   .HasForeignKey<PaymentAttempt>(p => p.DonationId);
            builder.OwnsOne(d => d.Amount, money =>
            {
                money.Property(m => m.Amount)
                     .HasColumnName("DonationAmount")
                     .HasColumnType("decimal(18, 2)");
                money.Property(m => m.Currency)
                     .HasColumnName("DonationCurrency")
                     .HasMaxLength(3);

            });    
        }
    }
}
