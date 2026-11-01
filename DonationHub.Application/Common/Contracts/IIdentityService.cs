using DonationHub.Application.Features.Auth.Refresh;

namespace DonationHub.Application.Common.Contracts
{
    public  interface IIdentityService
    {
        public Task<Guid> CreateDonorAsync(RegisterDonerCommand register);

        public Task<bool> EmailExistsAsync(string Email);

        public Task<UserDto?> FindUserByEmailAsync(string Email);

        public Task<bool> ValidateCredentialsAsync(string Email, string Password);


    }
}
