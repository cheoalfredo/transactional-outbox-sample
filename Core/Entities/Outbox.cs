using System.Security.Principal;

namespace OutboxPatternSample.Core.Entities
{
    public class Outbox : Entity
    {
        public string EventType { get; set; } = default!;
        public string Payload { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool Processed { get; set; } = false;
    }
}
