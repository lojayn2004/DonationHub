using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Features.Auth.Refresh;
using DonationHub.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace DonationHub.Infrastructure.ContractImplementations
{
    internal class IdentityService(UserManager<ApplicationUser> _userManager): IIdentityService
    {
        public async Task<Guid> CreateDonorAsync(RegisterDonerCommand register)
        {
            var appUser = ApplicationUser.Create(register.FullName, register.Email, register.UserName, register.PrefferedLanguage);
            var result = await _userManager.CreateAsync(appUser, register.Password);

            await _userManager.AddToRoleAsync(appUser, "Donor");
            return result.Succeeded ? appUser.Id : Guid.Empty;
        }

        public async Task<bool> EmailExistsAsync(string Email)
        {
            return (await _userManager.FindByEmailAsync(Email)) is not null;
        }

        public async Task<UserDto?> FindUserByEmailAsync(string Email)
        {
            var appUser = await _userManager.FindByEmailAsync(Email);
            if (appUser == null)
                return null;

            return new UserDto
            {
                Id = appUser.Id,
                Email = appUser.Email,
                UserName = appUser.UserName,
                FullName = appUser.FullName,
                PreferredLanguage = appUser.PreferredLanguage
            };
        }

        public async Task<bool> ValidateCredentialsAsync(string Email, string Password)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
                return false;

            return await _userManager.CheckPasswordAsync(user, Password);
        }
    }
}
