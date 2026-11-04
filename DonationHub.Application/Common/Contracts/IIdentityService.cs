using DonationHub.Application.Common.Dtos;
using DonationHub.Application.Features.Auth.Common;
using DonationHub.Application.Features.Common;

namespace DonationHub.Application.Common.Contracts
{
    public  interface IIdentityService
    {
        public Task<UserDto> CreateUserAsync(RegisterCommand register);

        Task<AuthResponse> GetAuthResponse(string Email);

        Task<AuthResponse> RefreshTokenAsync(string refreshToken);

        Task<bool> RevokeRefreshToken(string refreshToken);


        public Task<UserDto?> FindUserByEmailAsync(string Email);

        public Task<bool> ValidateCredentialsAsync(string Email, string Password);

        public Task<bool> EmailExistsAsync(string Email);
    }
}
