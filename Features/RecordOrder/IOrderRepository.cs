namespace OutboxPatternSample.Features.RecordOrder;

public interface IOrderRepository
{
    public Task StoreOrder(Order order);
}
