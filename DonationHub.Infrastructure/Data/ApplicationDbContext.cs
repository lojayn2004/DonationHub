using DonationHub.Application;
using DonationHub.Domain.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DonationHub.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply all IEntityTypeConfiguration<T> in this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            //foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            //{
            //    if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
            //    {
            //        // Create the expression: e => e.IsDeleted == false
            //        var parameter = Expression.Parameter(entityType.ClrType, "e");
            //        var body = Expression.Equal(
            //            Expression.Property(parameter, nameof(ISoftDelete.IsDeleted)),
            //            Expression.Constant(false));

            //        modelBuilder.Entity(entityType.ClrType)
            //            .HasQueryFilter(Expression.Lambda(body, parameter));
            //    }
            //}
        }
    }
}