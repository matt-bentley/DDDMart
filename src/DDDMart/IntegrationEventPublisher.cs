using DDDMart.Application.EventBus;
using DDDMart.Infrastructure;
using DDDMart.SharedKernel.Outbox.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DDDMart
{
    public class IntegrationEventPublisher<T> : BackgroundService where T : DbContextBase<T>
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger<IntegrationEventPublisher<T>> _logger;
        private const int TIMEOUT_SECONDS = 1;
        private readonly IServiceProvider _serviceProvider;

        public IntegrationEventPublisher(IEventBus eventBus,
            ILogger<IntegrationEventPublisher<T>> logger,
            IServiceProvider serviceProvider)
        {
            _eventBus = eventBus;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Checking for integration events every {timeout}s", TIMEOUT_SECONDS);
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TIMEOUT_SECONDS * 1000, stoppingToken);
                await PublishOutboxEventsAsync();               
            }
        }

        private async Task PublishOutboxEventsAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<T>();
                try
                {
                    var integrationEvents = context.OutboxIntegrationEvents.AsQueryable().ToList();

                    if (integrationEvents.Any())
                    {
                        _logger.LogInformation("Publishing {count} events from outbox", integrationEvents.Count);

                        foreach (var integrationEvent in integrationEvents)
                        {
                            await PublishIntegrationEventAsync(integrationEvent, context);
                        }
                        context.OutboxIntegrationEvents.RemoveRange(integrationEvents);
                        await context.CommitAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error publishing outbox events - {ex}", ex.ToString());
                }
            }
        }

        private async Task PublishIntegrationEventAsync(OutboxIntegrationEvent integrationEvent, T context)
        {
            var @event = context.EventMapper.Factory.Create(integrationEvent);
            await _eventBus.PublishAsync(@event);
        }
    }
}
