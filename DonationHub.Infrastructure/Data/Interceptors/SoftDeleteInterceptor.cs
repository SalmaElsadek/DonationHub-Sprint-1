using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DonationHub.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DonationHub.Infrastructure.Data.Interceptors
{
    public class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            if (eventData.Context is null) return result;

            foreach(var entry in eventData.Context.ChangeTracker.Entries())
            {
                //if (entry is null || entry.State != EntityState.Deleted || entry.Entity is not ISoftDeletable entity)
                //Pattern Matching
                if(entry is not {State:EntityState.Deleted,Entity:ISoftDeletable entity})    
                    continue;

                entry.State = EntityState.Modified;
                entity.Delete();//soft delete
            }
            return result;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            if (eventData.Context is null) return base.SavingChangesAsync(eventData, result, cancellationToken);

            foreach (var entry in eventData.Context.ChangeTracker.Entries())
            {
                if (entry is not { State: EntityState.Deleted, Entity: ISoftDeletable entity })
                    continue;

                entry.State = EntityState.Modified;
                entity.Delete();
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
