using DonationHub.Application.Common.Contracts;
using DonationHub.Domain.Results;
using DonationHub.Domain.Results.Errors;
using DonationHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DonationHub.Infrastructure.ContractImplementations
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

        public async Task<Result<int>> SavaChangesAsync(CancellationToken ct)
        {
            try
            {
                int result = await _dbContext.SaveChangesAsync(ct);
                return  Result<int>.Success(result);
            }
            catch(DbUpdateConcurrencyException ex)
            {
                return Result<int>.Failure(CommonErrors.ConcurrencyConflict);

            }
        }
    }
}
