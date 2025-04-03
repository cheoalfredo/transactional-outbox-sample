using OutboxPatternSample.Core.Event;

namespace OutboxPatternSample.Core.Entities
{
    public abstract class Entity 
    {
        public Guid Id { get; set; }        
        
    }
}
