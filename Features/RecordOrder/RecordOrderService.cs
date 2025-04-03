namespace OutboxPatternSample.Features.RecordOrder;

public class RecordOrderService(IOrderRepository orderRepository)
{
      
    public async Task RecordOrderAsync(Order order)
    {
        order.AddEvent(new OrderCreated(order.Id, DateTime.UtcNow));
        await orderRepository.StoreOrder(order);              
    }
}
