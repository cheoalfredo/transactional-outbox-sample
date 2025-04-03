namespace OutboxPatternSample.Core.Infrastructure
{
    public interface IUnitOfWork
    {
        public Task SaveAsync(CancellationToken? cancellationToken);
    }
}
