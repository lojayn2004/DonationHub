using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Common.Dtos;
using DonationHub.Application.Common.Results;
using DonationHub.Application.Features.Auth.Common;
using DonationHub.Application.Features.Common;
using DonationHub.Infrastructure.Data;
using DonationHub.Infrastructure.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace DonationHub.Infrastructure.ContractImplementations
{
    internal class IdentityService(UserManager<ApplicationUser> _userManager,
        IJwtUtils _jwtUtils): IIdentityService
    {
        public async Task<Result<UserDto>> CreateUserAsync(RegisterCommand register)
        {
            var appUser = ApplicationUser.Create(register.FullName, register.Email, register.UserName,
                register.PrefferedLanguage, DateTime.UtcNow);
            var result = await _userManager.CreateAsync(appUser, register.Password);
            if (!result.Succeeded)
            {
                var message = string.Join(" ", result.Errors.Select(error => error.Description));
                return Result<UserDto>.Failure(new Error("IdentityError", message, ErrorType.Validation));
            }

            var roleResult = await _userManager.AddToRoleAsync(appUser, register.Role);
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(appUser);
                var message = string.Join(" ", roleResult.Errors.Select(error => error.Description));
                return Result<UserDto>.Failure(new Error("IdentityError", message, ErrorType.UnExpected));
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

        public async Task<Result<AuthResponse>> GetAuthResponse(string Email)
        {
            var appUser = await _userManager.FindByEmailAsync(Email);
            if(appUser == null)
                return Result<AuthResponse>.Failure(Error.Unauthorized("UserNotFound", "Invalid user credentials."));
            var authResponse = new AuthResponse();
            authResponse.Token =  await _jwtUtils.GenerateJwtTokenAsync(appUser.Id);
            var activeRefreshToken = appUser.RefreshTokens.FirstOrDefault(t => t.IsActive);
            if (activeRefreshToken is not null)
            {   
                activeRefreshToken.RevokedOn = DateTime.UtcNow;
            }

            var refreshToken = _jwtUtils.GetRefreshToken();
            appUser.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(appUser);
            activeRefreshToken.ReplacedByTokenId = refreshToken.Id;

            await _userManager.UpdateAsync(appUser);
            authResponse.RefreshToken = refreshToken.Token!;
            authResponse.RefreshTokenExpiration = refreshToken.ExpiresAt;

            return Result<AuthResponse>.Success(authResponse);
        }

        public async Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken)
        {
            var refreshTokenHash = RefreshTokenUtils.HashRefreshToken(refreshToken);
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.TokenHash == refreshTokenHash));
            if (user == null)
                return Result<AuthResponse>.Failure(Error.Unauthorized("InvalidRefreshToken", "The provided refresh token is invalid or expired."));
            if(user.IsActive == false)
                return Result<AuthResponse>.Failure(Error.Forbidden("AccountLocked", "The account associated with the provided refresh token is locked."));

            var refreshTokenEntity = user.RefreshTokens.Single(t => t.TokenHash == refreshTokenHash);
            if (!refreshTokenEntity.IsActive)
                return Result<AuthResponse>.Failure(Error.Unauthorized("InvalidRefreshToken", "The provided refresh token is invalid or expired."));
            var authResponse = new AuthResponse();
            // refresh token revoke
            refreshTokenEntity.RevokedOn = DateTime.UtcNow;
            var newRefreshToken = _jwtUtils.GetRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);
            refreshTokenEntity.ReplacedByTokenId = newRefreshToken.Id;

            await _userManager.UpdateAsync(user);
            authResponse.RefreshToken = newRefreshToken.Token!;
            authResponse.RefreshTokenExpiration = newRefreshToken.ExpiresAt;
            // Generate new JWT token
            authResponse.Token = await _jwtUtils.GenerateJwtTokenAsync(user.Id);
 
            return Result<AuthResponse>.Success(authResponse);
        }

        public async Task<Result> RevokeRefreshToken(string refreshToken)
        {
            var refreshTokenHash = RefreshTokenUtils.HashRefreshToken(refreshToken);
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.TokenHash == refreshTokenHash));
            if (user == null)
                return Result.Failure(Error.Unauthorized("InvalidRefreshToken", "The provided refresh token is invalid or expired."));

            var refreshTokenEntity = user.RefreshTokens.Single(t => t.TokenHash == refreshTokenHash);
            if (!refreshTokenEntity.IsActive)
                return Result.Failure(Error.Unauthorized("InvalidRefreshToken", "The provided refresh token is invalid or expired."));
        
            // refresh token revoke
            refreshTokenEntity.RevokedOn = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            
            return Result.Success();
        }

        public async Task<bool> IsAccountLocked(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            return user != null && !user.IsActive;
        }
    }
}
