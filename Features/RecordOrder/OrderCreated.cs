using OutboxPatternSample.Core.Event;

namespace OutboxPatternSample.Features.RecordOrder;

public class OrderCreated(Guid orderId, DateTime created) : DomainEvent
{    
    public Guid OrderId => orderId;

    public DateTime Created => created;

}
