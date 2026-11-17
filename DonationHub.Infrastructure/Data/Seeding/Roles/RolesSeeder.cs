using DonationHub.Application.Common.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DonationHub.Infrastructure.Data.Seeding.Roles
{
    internal class RolesSeeder(ApplicationDbContext _appDbContext, RoleManager<IdentityRole<Guid>> _roleManager): IDataSeeder
    {
        public async Task SeedAllAsync(CancellationToken ct)
        {
            if(!await _roleManager.Roles.AnyAsync(ct))
            {
                var roles = InitialRolesData.Roles;
                foreach (var role in roles)
                {
                    await _roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }
            
        }
    }
}
