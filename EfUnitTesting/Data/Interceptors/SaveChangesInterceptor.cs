using EfUnitTesting.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EfUnitTesting.Data.Interceptors;

public class SaveChangesInterceptor : ISaveChangesInterceptor
{
    public InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        MoviesContext? context = eventData.Context as MoviesContext;

        if (context is null) 
            return result;

        ChangeTracker tracker = context.ChangeTracker;

        var deleteEntries = tracker.Entries<Genre>()
            .Where(entry => entry.State == EntityState.Deleted);

        foreach (var deleteEntry in deleteEntries)
        {
            deleteEntry.Property<bool>("Deleted").CurrentValue = true;
            deleteEntry.State = EntityState.Modified;
        }

        return result;
    }

    public ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
                                                                    CancellationToken cancellationToken = new CancellationToken())
    {
        return ValueTask.FromResult(SavingChanges(eventData, result));
    }
}