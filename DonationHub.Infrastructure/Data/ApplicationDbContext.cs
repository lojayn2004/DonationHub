using DonationHub.Domain.Models.Shared;
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

           
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (!typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                    continue;

                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.Property(parameter, nameof(ISoftDeletable.IsDeleted));
                var notDeleted = Expression.Equal(property, Expression.Constant(false));

                modelBuilder.Entity(entityType.ClrType)
                    .HasQueryFilter(Expression.Lambda(notDeleted, parameter));
            }
        }
    }
}