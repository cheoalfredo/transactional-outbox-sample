using OutboxPatternSample.Core.Entities;
using OutboxPatternSample.Core.Event;

namespace OutboxPatternSample.Features.RecordOrder;

public class Order : Entity, IEvent
{
    public Order()
    {
        this.Id = Guid.NewGuid();
    }

    public ICollection<DomainEvent> Events { get; set; } = [];

    public void AddEvent(DomainEvent domainEvent)
    {
        Events.Add(domainEvent);
    }
}
