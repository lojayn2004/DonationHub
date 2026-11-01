using DonationHub.Application.Common.Contracts;
using DonationHub.Infrastructure.ContractImplementations;
using DonationHub.Infrastructure.Data;
using DonationHub.Infrastructure.Data.Interceptors;
using DonationHub.Infrastructure.Data.Seeding.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;



namespace DonationHub.Infrastructure
{
    public static class InfrastructureServicesRegisteration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                    options
                         .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                         .AddInterceptors(new SoftDeleteInterceptor())
            );

            services
               .AddIdentityCore<ApplicationUser>(options =>
               {
                   options.Password.RequireDigit = true;
                   options.Password.RequireLowercase = true;
                   options.Password.RequireUppercase = true;
                   options.Password.RequireNonAlphanumeric = true;
                   options.Password.RequiredLength = 8;
                   options.User.RequireUniqueEmail = true;
               })
               .AddRoles<IdentityRole<Guid>>()
        
               .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJwtUtils, JwtUtils>();
            services.AddScoped<IDataSeeder, RolesSeeder>();
            services.AddScoped<IIdentityService, IdentityService>();
            return services;
        }
    
    
    
        public static async Task SeedDatabaseAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();
            foreach(var seeder in seeders)
            {
                await seeder.SeedAllAsync();
            }
        }

    }
}
