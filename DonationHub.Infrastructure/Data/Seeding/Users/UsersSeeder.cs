using DonationHub.Application.Common.Contracts;
using DonationHub.Shared.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


namespace DonationHub.Infrastructure.Data.Seeding.Users
{
    internal class UsersSeeder(UserManager<ApplicationUser> _userManager,
        IOptions<SeedAdminOptions> _adminOptions): IDataSeeder
    {
        public async Task SeedAllAsync()
        {
            if(!(await _userManager.Users.AnyAsync()))
            {
                var adminOptions = _adminOptions.Value;
                var adminUser = ApplicationUser.Create(adminOptions.FullName,
                    adminOptions.Email, adminOptions.Email ,"en");
                
                await _userManager.CreateAsync(adminUser, adminOptions.Password);
                await _userManager.AddToRoleAsync(adminUser, "Admin");
                
                var donorUser =  ApplicationUser.Create("Donor", "donor@example.com", "Donor0505", "en");


                await _userManager.CreateAsync(donorUser, "Donor@1234");
                await _userManager.AddToRoleAsync(donorUser, "Donor");


                var campaignManagerUser = ApplicationUser.Create("Campaign Manager", "campaignmanager@example.com", "CampaignManager0505", "en");
                
                var result = await _userManager.CreateAsync(campaignManagerUser, "CampaignManager@1234");

                foreach(var error in result.Errors)
                {
                    Console.WriteLine($"Error creating campaign manager user: {error.Description}");
                }
                await _userManager.AddToRoleAsync(campaignManagerUser, "CampaignManager");  


            }

        }
    }
}
