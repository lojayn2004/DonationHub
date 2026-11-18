using DonationHub.Domain.Results;

namespace DonationHub.Application.Common.Contracts
{
    public interface IUnitOfWork 
    {
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class;


        Task<Result<int>> SavaChangesAsync(CancellationToken ct);

    }
}
