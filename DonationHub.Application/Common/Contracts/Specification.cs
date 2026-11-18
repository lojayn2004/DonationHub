using System.Linq.Expressions;

namespace DonationHub.Application.Common.Contracts
{
    public abstract class Specification<TIn, Tout> where TIn : class where Tout: class 
    {
        public Expression<Func<TIn, bool>>? Criteria { get; protected init; }

        public List<Expression<Func<TIn, object>>> Includes { get; } = new();

        public Expression<Func<TIn, Tout>> Projection { get; protected set; } 

        protected Specification(Expression<Func<TIn, bool>>? criteria)
        {
            Criteria = criteria;
        }

        protected void AddInclude(Expression<Func<TIn, object>> include)
        {
            Includes.Add(include);
        }

        
    }
    
}