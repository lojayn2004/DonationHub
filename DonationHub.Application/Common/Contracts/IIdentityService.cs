using DonationHub.Application.Common.Dtos;
using DonationHub.Application.Features.Auth.Common;
using DonationHub.Domain.Results;

namespace DonationHub.Application.Common.Contracts
{
    public  interface IIdentityService
    {
         Task<Result<UserDto>> CreateUserAsync(RegisterCommand register, DateTime utcNow);

        Task<Result<AuthResponse>> GetAuthResponseAsync(string Email, DateTime utcNow);

        Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken, DateTime utcNow);

        Task<Result> RevokeRefreshTokenAsync(string refreshToken, DateTime utcNow);


         Task<UserDto?> FindUserByEmailAsync(string Email);

         Task<bool> ValidateCredentialsAsync(string Email, string Password);

         Task<bool> EmailExistsAsync(string Email);

        Task<bool> IsAccountLockedAsync(string Email);
    }
}
