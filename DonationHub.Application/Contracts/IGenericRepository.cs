namespace DonationHub.Application.Contracts
{
    public interface IGenericRepository<TEntity>
    {
        Task<IReadOnlyList<TEntity>> GetAllAsync();

        Task<TEntity?> GetByIdAsync(Guid id);

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);
    }
}
