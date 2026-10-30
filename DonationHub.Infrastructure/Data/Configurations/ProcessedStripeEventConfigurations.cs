using DonationHub.Domain.Models.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DonationHub.Infrastructure.Data.Configurations
{
    internal class ProcessedStripeEventConfigurations : IEntityTypeConfiguration<ProcessedStripeEvent>
    {
        public void Configure(EntityTypeBuilder<ProcessedStripeEvent> builder)
        {
            builder.HasKey(e => e.EventId);

            builder.Property(e => e.Type)
                   .HasMaxLength(600);
        }
    }
}
