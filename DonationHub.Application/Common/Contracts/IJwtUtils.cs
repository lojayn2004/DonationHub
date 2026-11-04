

using DonationHub.Domain.Models.RefreshToken;

namespace DonationHub.Application.Common.Contracts
{
    public interface IJwtUtils
    {
        Task<string> GenerateJwtTokenAsync(Guid UserId);

        RefreshToken GetRefreshToken();
    }
}

