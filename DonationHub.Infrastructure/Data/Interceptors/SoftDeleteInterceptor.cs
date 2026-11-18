using DonationHub.Domain.Models.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DonationHub.Infrastructure.Data.Interceptors
{
    public class SoftDeleteInterceptor: SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            if (eventData.Context is null)
                return result;

            foreach(var entry in eventData.Context.ChangeTracker.Entries())
            {
                if(entry is null || 
                   entry.State != EntityState.Deleted ||
                   !(entry.Entity is ISoftDeletable entity))
                {
                    continue;
                }

                entry.State = EntityState.Modified;
                entity.IsDeleted = true;
                entity.DeletedAtUtc = DateTime.UtcNow;
            }
                

            return base.SavingChanges(eventData, result);
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            ApplySoftDeletes(eventData);
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private static void ApplySoftDeletes(DbContextEventData eventData)
        {
            if (eventData.Context is null)
                return;

            foreach (var entry in eventData.Context.ChangeTracker.Entries<ISoftDeletable>())
            {
                if (entry.State != EntityState.Deleted)
                    continue;

                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedAtUtc = DateTime.UtcNow;
                MarkOwnedEntriesAsModified(entry);
            }
        }

        private static void MarkOwnedEntriesAsModified(EntityEntry entry)
        {
            foreach (var reference in entry.References)
            {
                var targetEntity = reference.TargetEntry as EntityEntry;
                if (targetEntity is not null && targetEntity.Metadata.IsOwned())
                {
                    targetEntity.State = EntityState.Modified;
                    // nested owned types
                    MarkOwnedEntriesAsModified(targetEntity); 
                }
            }
        }

    }
}
