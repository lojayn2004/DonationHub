namespace DonationHub.Application.Common.Contracts
{
    public interface IGenericRepository<TEntity> where TEntity: class
    {
        Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct);

        Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct);

        Task<TEntity?> GetByIdAsync(
            Guid id,
            Specification<TEntity, TEntity> specification,
            CancellationToken ct);

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);
    }
}
