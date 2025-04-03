namespace OutboxPatternSample.Core.Event
{
    public interface IEvent
    {
        public ICollection<DomainEvent> Events { get; set; }

        void AddEvent(DomainEvent domainEvent);
    }
}
