using DonationHub.Application.Common.Dtos;
using DonationHub.Application.Features.Auth.Common;
using DonationHub.Application.Features.Common;

namespace DonationHub.Application.Common.Contracts
{
    public  interface IIdentityService
    {
         Task<UserDto> CreateUserAsync(RegisterCommand register);

        Task<AuthResponse> GetAuthResponse(string Email);

        Task<AuthResponse> RefreshTokenAsync(string refreshToken);

        Task<bool> RevokeRefreshToken(string refreshToken);


         Task<UserDto?> FindUserByEmailAsync(string Email);

         Task<bool> ValidateCredentialsAsync(string Email, string Password);

         Task<bool> EmailExistsAsync(string Email);

        Task<bool> IsAccountLocked(string Email);
    }
}
