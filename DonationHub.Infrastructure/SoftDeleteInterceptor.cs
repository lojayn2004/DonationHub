using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using DonationHub.Application;

namespace DonationHub.Infrastructure
{
    internal class SoftDeleteInterceptor: SaveChangesInterceptor
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
                   !(entry.Entity is ISoftDelete entity))
                {
                    continue;
                }

                entry.State = EntityState.Modified;
                entity.IsDeleted = true;
                entity.DeletedAtUtc = DateTime.UtcNow;
            }
                

            return base.SavingChanges(eventData, result);
        }

    }
}
