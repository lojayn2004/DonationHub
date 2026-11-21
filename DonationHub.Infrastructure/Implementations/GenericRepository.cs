using DonationHub.Application.Common.Contracts;
using DonationHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DonationHub.Infrastructure.ContractImplementations
{
    internal class GenericRepository<TEntity>(ApplicationDbContext _applicationDbContext):
        IGenericRepository<TEntity> where TEntity: class
    {
        public void Add(TEntity entity)
        {
            _applicationDbContext.Set<TEntity>().Add(entity);
            
        }

        public void Delete(TEntity entity)
        {
            _applicationDbContext.Set<TEntity>().Remove(entity);
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct)
        {
            return await _applicationDbContext.Set<TEntity>().ToListAsync(ct);
        }

        public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _applicationDbContext.Set<TEntity>().FindAsync(id,ct);
        }

        public async Task<TEntity?> GetByIdAsync(
            Guid id,
            Specification<TEntity, TEntity> specification,
            CancellationToken ct)
        {
            IQueryable<TEntity> query = _applicationDbContext.Set<TEntity>();

            if (specification.Criteria is not null)
                query = query.Where(specification.Criteria);

            foreach (var include in specification.Includes)
                query = query.Include(include);

            query = query.Where(entity => EF.Property<Guid>(entity, "Id") == id);

            return await query.Select(specification.Projection).FirstOrDefaultAsync(ct);
        }

        public void Update(TEntity entity)
        {
            _applicationDbContext.Set<TEntity>().Update(entity);
        }
    }
}
