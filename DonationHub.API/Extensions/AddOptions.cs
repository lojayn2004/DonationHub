using DonationHub.Shared;

namespace DonationHub.API.Extensions
{
    public static class AddOptions
    {
        public static IServiceCollection AddOptionsConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<JwtOptions>()
                    .BindConfiguration("JwtOptions")
                    .ValidateDataAnnotations()
                    .ValidateOnStart();


            //services.AddOptions<DonationOptions>()
            //    .BindConfiguration("DonationOptions")
            //    .ValidateDataAnnotations()
            //    .ValidateOnStart();

            services.AddOptions<SeedAdminOptions>()
                .BindConfiguration("AdminSeedOptions")
                .ValidateDataAnnotations()
                .ValidateOnStart();
            return services;
        }
    }
}
