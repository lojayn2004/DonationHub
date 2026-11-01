using DonationHub.Application.Common.Contracts;
using DonationHub.Infrastructure.Data;

namespace DonationHub.Infrastructure
{
    internal class UnitOfWork(ApplicationDbContext _dbContext) : IUnitOfWork
    {
        private Dictionary<string, object> _repos = new();
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity: class
        {
            string repoName = typeof(TEntity).Name;
            if (!_repos.ContainsKey(repoName))
                _repos[repoName] = new GenericRepository<TEntity>(_dbContext);
            return (IGenericRepository<TEntity>) _repos[repoName];
        }

        public async Task<int> SavaChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
