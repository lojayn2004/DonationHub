using DonationHub.Application.Common.Contracts;
using DonationHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DonationHub.Infrastructure
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

        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
        {
            return await _applicationDbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(Guid id)
        {
            return await _applicationDbContext.Set<TEntity>().FindAsync(id);
        }

        public void Update(TEntity entity)
        {
            _applicationDbContext.Set<TEntity>().Update(entity);
        }
    }
}
