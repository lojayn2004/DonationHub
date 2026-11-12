using DonationHub.Application.Common.Contracts;
using DonationHub.Infrastructure.Data;
using DonationHub.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;

namespace DonationHub.Infrastructure.Implementations
{
    internal class Query<TIn, TOut>(ApplicationDbContext _dbContext): IQuery<TIn, TOut> where TIn: class
                                                                                        where TOut: class
    {
        public async Task<IReadOnlyList<TOut>> GetAllAsync(Specification<TIn, TOut> spec) {

            var query = _dbContext.Set<TIn>().AsNoTracking();
         
            return await QueryEvaluator.CreateQuery<TIn, TOut>(query, spec).ToListAsync();
          

        }

        public async Task<TOut?> GetByIdAsync(Guid Id, Specification<TIn, TOut> spec)
        {
            var query = _dbContext.Set<TIn>().AsNoTracking();

            return await QueryEvaluator.CreateQuery<TIn, TOut>(query, spec).FirstOrDefaultAsync();
        }
    }
}
