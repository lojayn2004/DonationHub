using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Common.Dtos;
using DonationHub.Application.Features.Auth.Common;
using DonationHub.Application.Features.Common;
using DonationHub.Domain.Exceptions;
using DonationHub.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace DonationHub.Infrastructure.ContractImplementations
{
    internal class IdentityService(UserManager<ApplicationUser> _userManager,
        IJwtUtils _jwtUtils): IIdentityService
    {
        public async Task<UserDto> CreateUserAsync(RegisterCommand register)
        {
            var appUser = ApplicationUser.Create(register.FullName, register.Email, register.UserName,
                register.PrefferedLanguage, DateTime.UtcNow);
            var result = await _userManager.CreateAsync(appUser, register.Password);

            await _userManager.AddToRoleAsync(appUser, register.Role);
            return  new UserDto
            {
                Id = appUser.Id,
                Email = appUser.Email,
                UserName = appUser.UserName,
                FullName = appUser.FullName,
                PreferredLanguage = appUser.PreferredLanguage
            };
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

        public async Task<AuthResponse> GetAuthResponse(string Email)
        {
            var appUser = await _userManager.FindByEmailAsync(Email);
            if(appUser == null)
                throw new Exception("User not found");
            var authResponse = new AuthResponse();
            authResponse.Token =  await _jwtUtils.GenerateJwtTokenAsync(appUser.Id);

            if (appUser.RefreshTokens.Any(t => t.IsActive))
            {
                var activeRefreshToken = appUser.RefreshTokens.First(t => t.IsActive);
                authResponse.RefreshToken = activeRefreshToken.Token;
                authResponse.RefreshTokenExpiration = activeRefreshToken.ExpiresAt;
            }
            else
            {
                var refreshToken = _jwtUtils.GetRefreshToken();
                appUser.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(appUser);
                authResponse.RefreshToken = refreshToken.Token;
                authResponse.RefreshTokenExpiration = refreshToken.ExpiresAt;
            }
            return authResponse;
        }

        public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));
            if (user == null)
                throw new Exception("Invalid refresh token");
            if(user.IsActive == false)
                throw new ForbiddenException("User account is inactive");

            var refreshTokenEntity = user.RefreshTokens.Single(t => t.Token == refreshToken);
            if (!refreshTokenEntity.IsActive)
                throw new Exception("token is not active");
            var authResponse = new AuthResponse();
            // refresh token revoke
            refreshTokenEntity.RevokedOn = DateTime.UtcNow;

            var newRefreshToken = _jwtUtils.GetRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);
            authResponse.RefreshToken = newRefreshToken.Token;
            authResponse.RefreshTokenExpiration = newRefreshToken.ExpiresAt;
            // Generate new JWT token
            authResponse.Token = await _jwtUtils.GenerateJwtTokenAsync(user.Id);
 
            return authResponse;
        }

        public async Task<bool> RevokeRefreshToken(string refreshToken)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));
            if (user == null)
                throw new Exception("Invalid refresh token");

            var refreshTokenEntity = user.RefreshTokens.Single(t => t.Token == refreshToken);
            if (!refreshTokenEntity.IsActive)
                throw new Exception("token is not active");
        
            // refresh token revoke
            refreshTokenEntity.RevokedOn = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            
            return true;
        }

        public async Task<bool> IsAccountLocked(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            return user != null && !user.IsActive;
        }
    }
}
