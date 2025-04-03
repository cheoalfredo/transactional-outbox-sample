
using Microsoft.EntityFrameworkCore;
using OutboxPatternSample.Core.Entities;
using OutboxPatternSample.Core.Infrastructure.DataAccess;
using RabbitMQ.Client;
using System.Threading.Channels;

namespace OutboxPatternSample.Features.ProcessOrder
{
    public class ProcessOrderService(
        IServiceProvider _services,
        ILogger<ProcessOrderService> _logger,
        ConnectionFactory factory) : BackgroundService
    {

        private IConnection? _connection;
        private IChannel? _channel;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _connection = await factory.CreateConnectionAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);
            await _channel.ExchangeDeclareAsync("main", ExchangeType.Direct, cancellationToken: stoppingToken);
            await _channel.QueueDeclareAsync("outbox", false, false, false, null, cancellationToken: stoppingToken);
            await _channel.QueueBindAsync("outbox", "main", "transactional:outbox", cancellationToken: stoppingToken);

            using PeriodicTimer timer = new(TimeSpan.FromSeconds(10));

            try
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await FetchAndPushOutboxMessages(stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Timed Hosted Service is stopping.");
            }
        }

        private async Task FetchAndPushOutboxMessages(CancellationToken token)
        {
            using var scope = _services.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<OutboxContext>();
            // traemos las entradas del outbox que no han sido procesadas
            var orders = repo.Set<Outbox>().Where(o => !o.Processed).ToList();
            _logger.LogInformation("Eventos para procesar en Outbox: {Count}", orders.Count);

            if (_channel is not null && orders.Count > 0)
            {
                foreach (var item in orders)
                {
                    var payload = System.Text.Encoding.UTF8.GetBytes(item.Payload);
                    await _channel.BasicPublishAsync("main", "transactional:outbox", payload, token);
                    // una vez entregado el mensaje, lo marcamos como procesado
                    await repo.Set<Outbox>()
                        .Where(o => o.Id == item.Id)
                        .ExecuteUpdateAsync(e => e.SetProperty(x => x.Processed, true), cancellationToken: token);
                }
                
            }
        }
    }
}
