using DonationHub.Domain.Models.IdempotencyRecord;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DonationHub.Infrastructure.Data.Configurations
{
    internal class IdempotencyRecordConfigurations : IEntityTypeConfiguration<IdempotencyRecord>
    {
   
        public void Configure(EntityTypeBuilder<IdempotencyRecord> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.RequestHash)
                   .IsRequired();

            builder.Property(u => u.ResponseBody)
                  .IsRequired();

        }
    }
}
