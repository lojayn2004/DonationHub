

namespace DonationHub.Application.Common.Contracts
{
    public interface IJwtUtils
    {
        Task<string> GenerateJwtTokenAsync(Guid UserId);
    }
}

