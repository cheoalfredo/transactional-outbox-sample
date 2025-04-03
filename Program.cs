using Microsoft.EntityFrameworkCore;
using OutboxPatternSample.Core.Infrastructure;
using OutboxPatternSample.Core.Infrastructure.Adapters;
using OutboxPatternSample.Core.Infrastructure.DataAccess;
using OutboxPatternSample.Features.ProcessOrder;
using OutboxPatternSample.Features.RecordOrder;
using RabbitMQ.Client;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<OutboxContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument();

var config = builder.Configuration;


builder.Services.AddSingleton((svc) =>
{    
    return new ConnectionFactory
    {
        HostName = config.GetValue<string>("RabbitMQ:Host") ?? "localhost",
        UserName = config.GetValue<string>("RabbitMQ:Username") ?? "guest",
        Password = config.GetValue<string>("RabbitMQ:Password") ?? "guest",
        Port = int.Parse(config.GetValue<string>("RabbitMQ:Port") ?? "5672"),
        VirtualHost = "/",
        ClientProvidedName = "app:outbox:producer"
    };
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<RecordOrderService>();

builder.Services.AddHostedService<ProcessOrderService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<OutboxContext>();
    await context.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.MapGroup("/api/order").MapRecordOrder();

await app.RunAsync();
