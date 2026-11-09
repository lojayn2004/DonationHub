using DonationHub.Application.Common.Dtos;
using DonationHub.Application.Common.Results;
using DonationHub.Application.Features.Auth.Common;
using DonationHub.Application.Features.Common;

namespace DonationHub.Application.Common.Contracts
{
    public  interface IIdentityService
    {
         Task<Result<UserDto>> CreateUserAsync(RegisterCommand register);

        Task<Result<AuthResponse>> GetAuthResponseAsync(string Email);

        Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken);

        Task<Result> RevokeRefreshTokenAsync(string refreshToken);


         Task<UserDto?> FindUserByEmailAsync(string Email);

         Task<bool> ValidateCredentialsAsync(string Email, string Password);

         Task<bool> EmailExistsAsync(string Email);

        Task<bool> IsAccountLockedAsync(string Email);
    }
}
