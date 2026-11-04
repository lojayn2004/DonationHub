using DonationHub.Application.Common.Contracts;
using DonationHub.Domain.Models.RefreshToken;
using DonationHub.Infrastructure.Data;
using DonationHub.Infrastructure.Implementations;
using DonationHub.Shared.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DonationHub.Infrastructure.ContractImplementations
{
    public class JwtUtils(UserManager<ApplicationUser> _userManager, 
        IOptions<JwtOptions> _jwtOptions): IJwtUtils
    {
        public  async Task<string> GenerateJwtTokenAsync(Guid UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId.ToString());
            var claims = await GetClaimsAsync(user, _userManager);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.SecretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                issuer: _jwtOptions.Value.Issuer,
                audience: _jwtOptions.Value.Audience,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.Value.ExpiryMinutes)
                );
            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            
            
            return token;
        }

        public RefreshToken GetRefreshToken()
        {
            var refreshToken = RefreshTokenUtils.GenerateRefreshToken();
            return new RefreshToken
            {
                Token = refreshToken,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(_jwtOptions.Value.RefreshTokenExpiryDays) 
            };
        }

        private static async Task<List<Claim>> GetClaimsAsync(ApplicationUser user, UserManager<ApplicationUser> _userManager)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
                claims.Add(new Claim("Role", role));
            }
            return claims;
        }

      
    }

}
