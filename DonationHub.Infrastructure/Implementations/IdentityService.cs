using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Common.Dtos;
using DonationHub.Application.Features.Auth.Common;
using DonationHub.Domain.Results;
using DonationHub.Domain.Results.Errors;
using DonationHub.Infrastructure.Data;
using DonationHub.Infrastructure.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace DonationHub.Infrastructure.ContractImplementations
{
    internal class IdentityService(UserManager<ApplicationUser> _userManager,
        IJwtUtils _jwtUtils): IIdentityService
    {
        public async Task<Result<UserDto>> CreateUserAsync(RegisterCommand register, DateTime utcNow)
        {
            var appUser = ApplicationUser.Create(register.FullName, register.Email, register.UserName,
                register.PrefferedLanguage, utcNow);
            var result = await _userManager.CreateAsync(appUser, register.Password);
            if (!result.Succeeded)
            {
                // TODO: enhance this to return error detais
                var message = string.Join(" ", result.Errors.Select(error => error.Description));
                return Result<UserDto>.Failure(ValidationErrors.Falied);
            }

            var roleResult = await _userManager.AddToRoleAsync(appUser, register.Role);
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(appUser);
                var message = string.Join(" ", roleResult.Errors.Select(error => error.Description));
                return Result<UserDto>.Failure(ValidationErrors.Falied);
            }

            return Result<UserDto>.Success(new UserDto
            {
                Id = appUser.Id,
                Email = appUser.Email,
                UserName = appUser.UserName,
                FullName = appUser.FullName,
                PreferredLanguage = appUser.PreferredLanguage
            });
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

        public async Task<Result<AuthResponse>> GetAuthResponseAsync(string Email, DateTime utcNow)
        {
            var appUser = await _userManager.FindByEmailAsync(Email);
            if(appUser == null)
                return Result<AuthResponse>.Failure(AuthErrors.InValidCredentials);
            var authResponse = new AuthResponse();
            authResponse.Token =  await _jwtUtils.GenerateJwtTokenAsync(appUser.Id);
            var activeRefreshToken = appUser.RefreshTokens.FirstOrDefault(t => t.IsActive(utcNow));
          

            var refreshToken = _jwtUtils.GetRefreshToken();
            appUser.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(appUser);
            if(activeRefreshToken is not null) 
                activeRefreshToken.RevokeToken(utcNow, refreshToken.Id);

            await _userManager.UpdateAsync(appUser);
            authResponse.RefreshToken = refreshToken.Token!;
            authResponse.RefreshTokenExpiration = refreshToken.ExpiresAt;

            return Result<AuthResponse>.Success(authResponse);
        }

        public async Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken, DateTime utcNow)
        {
            var refreshTokenHash = RefreshTokenUtils.HashRefreshToken(refreshToken);
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.TokenHash == refreshTokenHash));
            if (user == null)
                return Result<AuthResponse>.Failure(AuthErrors.RefreshTokenInvalid);
            if(user.IsActive == false)
                return Result<AuthResponse>.Failure(AuthErrors.AccountDeactivated);

            var refreshTokenEntity = user.RefreshTokens.Single(t => t.TokenHash == refreshTokenHash);
            if (!refreshTokenEntity.IsActive(utcNow))
                return Result<AuthResponse>.Failure(AuthErrors.RefreshTokenInvalid);
            var authResponse = new AuthResponse();
            // refresh token revoke
            
            var newRefreshToken = _jwtUtils.GetRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);
            refreshTokenEntity.RevokeToken(utcNow, newRefreshToken.Id);
            

            await _userManager.UpdateAsync(user);
            authResponse.RefreshToken = newRefreshToken.Token!;
            authResponse.RefreshTokenExpiration = newRefreshToken.ExpiresAt;
            // Generate new JWT token
            authResponse.Token = await _jwtUtils.GenerateJwtTokenAsync(user.Id);
 
            return Result<AuthResponse>.Success(authResponse);
        }

        public async Task<Result> RevokeRefreshTokenAsync(string refreshToken, DateTime utcNow)
        {
            var refreshTokenHash = RefreshTokenUtils.HashRefreshToken(refreshToken);
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.TokenHash == refreshTokenHash));
            if (user == null)
                return Result.Failure(AuthErrors.RefreshTokenInvalid);

            var refreshTokenEntity = user.RefreshTokens.Single(t => t.TokenHash == refreshTokenHash);
            if (!refreshTokenEntity.IsActive(utcNow))
                return Result.Failure(AuthErrors.RefreshTokenInvalid);
        
            // refresh token revoke
            refreshTokenEntity.RevokeToken(utcNow, null);
            await _userManager.UpdateAsync(user);
            
            return Result.Success();
        }

        public async Task<bool> IsAccountLockedAsync(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            return user != null && !user.IsActive;
        }
    }
}
