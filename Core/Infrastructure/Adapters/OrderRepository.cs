using Microsoft.EntityFrameworkCore;
using OutboxPatternSample.Core.Infrastructure.DataAccess;
using OutboxPatternSample.Features.RecordOrder;

namespace OutboxPatternSample.Core.Infrastructure.Adapters;

public class OrderRepository(OutboxContext _context) : IOrderRepository
{
    public async Task StoreOrder(Order order)
    {
        await _context.Set<Order>().AddAsync(order);
    }
}
