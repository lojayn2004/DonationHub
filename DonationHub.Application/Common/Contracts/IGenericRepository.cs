namespace DonationHub.Application.Common.Contracts
{
    public interface IGenericRepository<TEntity>
    {
        Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct);

        Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct);

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);
    }
}
