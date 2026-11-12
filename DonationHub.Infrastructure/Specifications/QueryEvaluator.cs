

using DonationHub.Application.Common.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DonationHub.Infrastructure.Specifications
{
    internal static class QueryEvaluator
    {
        public static IQueryable<TOut> CreateQuery<TIn, TOut>(IQueryable<TIn> inputQuery, Specification<TIn, TOut> specification) 
            where TIn: class
           where TOut : class
        {
           
            if (specification.Criteria is not null)
                inputQuery = inputQuery.Where(specification.Criteria);

            foreach (var include in specification.Includes)
            {
                inputQuery = inputQuery.Include(include);
            }

            var projectedQuery = inputQuery.Select(specification.Projection);
            return projectedQuery;

        }
    }
}
