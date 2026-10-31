namespace DonationHub.Application.Common.Contracts
{
    public interface IUnitOfWork 
    {
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class;


        Task<int> SavaChangesAsync();

    }
}
