using MediatR;
using OutboxPatternSample.Core.Infrastructure;

namespace OutboxPatternSample.Features.RecordOrder;

public class RecordOrderCommand : IRequest
{
  
}

public record RecordOrderHandler(RecordOrderService orderService, IUnitOfWork unitOfWork) 
    : IRequestHandler<RecordOrderCommand>
{

    public async Task Handle(RecordOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order(); // La info de la orden se extrae del request
        await orderService.RecordOrderAsync(order);
        await unitOfWork.SaveAsync(cancellationToken);
    }
}



