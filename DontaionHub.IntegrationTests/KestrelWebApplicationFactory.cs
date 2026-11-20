using DonationHub.Application.Common.Contracts;
using DonationHub.Infrastructure.Data;
using DonationHub.Infrastructure.Data.Interceptors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace DontaionHub.IntegrationTests
{
    

    public class KestrelWebApplicationFactory : WebApplicationFactory<Program>
    {
        private IHost? _realHost;
      

        public string BaseUrl { get; private set; } = "";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

        
            builder.UseSetting("AdminSeedOptions:Email", "admin@test.com");
            builder.UseSetting("AdminSeedOptions:Password", "Test@12345");
            builder.UseSetting("AdminSeedOptions:FullName", "admin");
            
            builder.UseSetting("JwtOptions:SecretKey", "a-long-test-key-at-least-32-characters!!");
            builder.UseSetting("JwtOptions:Issuer", "DonationHub");
            builder.UseSetting("JwtOptions:ExpiryMinutes", "60");
            builder.UseSetting("JwtOptions:RefreshTokenExpiryDays", "7");
            builder.UseSetting("JwtOptions:Audience", "DontaionHubUsers");


           

            builder.ConfigureServices(services =>
            {

                var connectionString = Environment.GetEnvironmentVariable("TEST_DB_CONNECTION");



                services.AddDbContext<ApplicationDbContext>(o => o
                   .UseSqlServer(connectionString ?? 
                   "Server=.;Database=DonationHubTestDb;Trusted_Connection=True;TrustServerCertificate=True")
                   .AddInterceptors(new SoftDeleteInterceptor()));
            });
        }
        
        protected override IHost CreateHost(IHostBuilder builder)
        {
            // In-memory host (WebApplicationFactory requires this to be returned)
            var testHost = builder.Build();

            // Real host on a random free port 
            builder.ConfigureWebHost(wb =>
                wb.UseKestrel(o => o.Listen(IPAddress.Loopback, 0)));
            _realHost = builder.Build();
            _realHost.Start();

            
            using (var scope = _realHost.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.EnsureCreated();
                
                var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();
                foreach (var seeder in seeders)
                {
                     
                     seeder.SeedAllAsync(CancellationToken.None).GetAwaiter().GetResult();
                }
            }



          
            BaseUrl = _realHost.Services.GetRequiredService<IServer>()
                .Features.Get<IServerAddressesFeature>()!.Addresses.First();

            return testHost;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _realHost?.Dispose();
               
            }
        }
    }
}
