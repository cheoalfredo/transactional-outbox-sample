using Microsoft.EntityFrameworkCore;
using OutboxPatternSample.Core.Entities;
using OutboxPatternSample.Core.Event;
using OutboxPatternSample.Features.RecordOrder;
using System.Text.Json;

namespace OutboxPatternSample.Core.Infrastructure.DataAccess;

public class OutboxContext(DbContextOptions<OutboxContext> options) : DbContext(options)
{

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (modelBuilder is null)
        {
            return;
        }
       
        modelBuilder.Entity<Order>();

        modelBuilder.Entity<Outbox>();

        // Ubicamos todas las clases que implementen la interfaz IEvent y excluimos de la persistenicia los eventos
        // No necesitamos persistir los eventos en la entidad/agregado, estos van al outbox
        foreach (var eType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = eType.ClrType;
            if (typeof(IEvent).IsAssignableFrom(clrType))
            {
                modelBuilder.Entity(clrType).Ignore(nameof(IEvent.Events));
            }
        }

        modelBuilder.Model.GetEntityTypes().Where(e => typeof(Entity).IsAssignableFrom(e.ClrType))
            .ToList()
            .ForEach(e =>
            {
                modelBuilder.Entity(e.ClrType).Property<DateTime>("CreatedOn");
                modelBuilder.Entity(e.ClrType).Property<DateTime>("LastModifiedOn");
            });


        base.OnModelCreating(modelBuilder);
    }
}

