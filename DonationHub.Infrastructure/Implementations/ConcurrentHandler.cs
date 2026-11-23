using DonationHub.Application.Common.Contracts;
using DonationHub.Domain.Models.Shared;
using DonationHub.Infrastructure.Data;

namespace DonationHub.Infrastructure.Implementations
{
    internal class ConcurrentHandler<TEntity>(ApplicationDbContext _dbContext): 
        IConcurrencyHandler<TEntity> where TEntity : class, IConcurrency
    {
        public void HandleConcurrentUpdate(TEntity entity, byte[] RowVersion)
        {
            _dbContext.Entry<TEntity>(entity).Property(c => c.rowVersion).OriginalValue = RowVersion;

        }
    }
}
