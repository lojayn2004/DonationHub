using Microsoft.EntityFrameworkCore;

namespace DonationHub.Infrastructure.Data
{
    internal class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options) :base(options)
        {

        }
       
       protected override void OnModelCreating(ModelBuilder modelBuilder)
       {
            // apply configuration from assembly 
       }

    }
}
