using DDDMart.Application.EventBus;
using DDDMart.Ordering.Application.IntegrationEvents.Handlers;
using DDDMart.Payments.Application.IntegrationEvents.Handlers;
using Microsoft.Extensions.Hosting;

namespace DDDMart
{
    public class IntegrationEventsService : IHostedService
    {
        private readonly IEventBus _eventBus;

        public IntegrationEventsService(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _eventBus.Subscribe<Payments.Application.IntegrationEvents.OrderSubmittedIntegrationEvent, OrderSubmittedIntegrationEventHandler>();
            _eventBus.Subscribe<Ordering.Application.IntegrationEvents.InvoicePaidIntegrationEvent, InvoicePaidIntegrationEventHandler>();
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _eventBus.DisposeAsync();
        }
    }
}
