using MediatR;
using System.Net;

namespace OutboxPatternSample.Features.RecordOrder;

public static class RecordOrderApi
{
    public static RouteGroupBuilder MapRecordOrder(this RouteGroupBuilder builder)
    {
        // Vía mediador
        builder.MapPost("/", async (IMediator pipeline, RecordOrderCommand request) =>
        {
            await pipeline.Send(request);            
        })
        .WithName("Record a voter");
       

        return builder.WithOpenApi();
    }

}
