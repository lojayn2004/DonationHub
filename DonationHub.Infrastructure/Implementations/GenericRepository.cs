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

        public void Update(TEntity entity)
        {
            _applicationDbContext.Set<TEntity>().Update(entity);
        }
    }
}
