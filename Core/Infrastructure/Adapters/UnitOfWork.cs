using OutboxPatternSample.Core.Entities;
using OutboxPatternSample.Core.Event;
using OutboxPatternSample.Core.Infrastructure.DataAccess;
using System.Text.Json;

namespace OutboxPatternSample.Core.Infrastructure.Adapters;

public class UnitOfWork(OutboxContext _context) : IUnitOfWork
{
    public async Task SaveAsync(CancellationToken? cancellationToken)
    {
        cancellationToken ??= new CancellationTokenSource().Token;
        _context.ChangeTracker.DetectChanges();

        var domainEventEntities = _context.ChangeTracker.Entries()
            .Select(entityEntry => entityEntry.Entity)
            .Where(entityEntry =>  typeof(IEvent).IsAssignableFrom(entityEntry.GetType())).ToArray();

        for (int i = 0; i < domainEventEntities.Count(); ++i)
        {
            var entry = (IEvent)domainEventEntities[i];
            var events = entry.Events.ToArray();
            entry.Events.Clear();           
            foreach (var evt in events)
            {
                await _context.Set<Outbox>().AddAsync(new Outbox
                {
                    EventType = evt.GetType().Name,
                    Payload = JsonSerializer.Serialize(evt, evt.GetType()),
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        await _context.SaveChangesAsync(cancellationToken.Value);

    }

   
}
