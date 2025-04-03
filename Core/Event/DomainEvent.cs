using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace OutboxPatternSample.Core.Event;

public class DomainEvent
{
    public DomainEvent()
    {
        On = DateTime.UtcNow;
    }

    public DateTime On { get; set; }
   
}
