
using DonationHub.Application.Contracts;

namespace DonationHub.Application
{
    public interface IUnitOfWork 
    {
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class;


        Task<int> SavaChangesAsync();

    }
}
