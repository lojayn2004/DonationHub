using DonationHub.Application.Common.Contracts;
using DonationHub.Infrastructure.ContractImplementations;
using DonationHub.Infrastructure.Data;
using DonationHub.Infrastructure.Data.Interceptors;
using DonationHub.Infrastructure.Data.Seeding.Categories;
using DonationHub.Infrastructure.Data.Seeding.Roles;
using DonationHub.Infrastructure.Data.Seeding.Users;
using DonationHub.Shared.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;



namespace DonationHub.Infrastructure
{
    public static class InfrastructureServicesRegisteration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, 
            IConfiguration configuration)
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


            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var jwtOptions = configuration
                        .GetSection("JwtOptions")
                        .Get<JwtOptions>() ?? throw new InvalidOperationException("JWT options not configured");

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,

                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                    };
                });

            services.AddScoped<IDataSeeder, CategoriesSeeder>();
            services.AddScoped<IDataSeeder, RolesSeeder>();
            services.AddScoped<IDataSeeder, UsersSeeder>();

            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJwtUtils, JwtUtils>();
            return services;
        }
    
    
    
        public static async Task SeedDatabaseAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();
            foreach(var seeder in seeders)
            {
                Console.WriteLine($"Seeding data using {seeder.GetType().Name}...");
                await seeder.SeedAllAsync();
            }
        }

    }
}
