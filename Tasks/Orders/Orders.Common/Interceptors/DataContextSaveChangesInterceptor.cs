using System.Data.Common;
using MassTransit.Internals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Orders.Common.Abstractions;

namespace Orders.Common.Interceptors;

public class DataContextSaveChangesInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        var outboxMessages = GetOutboxMessages(eventData).ToList();
        if(outboxMessages.Count == 0)
            return base.SavingChanges(eventData, result);
        
        eventData.Context!.AddRange(outboxMessages);
        
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var outboxMessages = GetOutboxMessages(eventData).ToList();
        if(outboxMessages.Count == 0)
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        
        eventData.Context!.AddRange(outboxMessages);
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static IEnumerable<IOutboxMessage> GetOutboxMessages(DbContextEventData eventData)
    {
        return eventData.Context!.ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Added)
            .Select(entity => entity.Entity as IOutboxMessageEntity)
            .Select(message => message!.Message);
    }
}