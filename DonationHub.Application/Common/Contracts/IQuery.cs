

namespace DonationHub.Application.Common.Contracts
{
    public interface IQuery<TIn, Tout> where TIn : class 
        where Tout: class
    {
        Task<Tout?> GetByIdAsync(Guid Id, Specification<TIn, Tout> spec);

        Task<IReadOnlyList<Tout>> GetAllAsync(Specification<TIn, Tout> spec);

    }
}
