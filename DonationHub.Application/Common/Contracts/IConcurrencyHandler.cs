using DonationHub.Domain.Models.Shared;

namespace DonationHub.Application.Common.Contracts
{
    public interface IConcurrencyHandler<TEntity> where TEntity : IConcurrency
    {
        void HandleConcurrentUpdate(TEntity entity, byte[] RowVersion); 
    }
}
