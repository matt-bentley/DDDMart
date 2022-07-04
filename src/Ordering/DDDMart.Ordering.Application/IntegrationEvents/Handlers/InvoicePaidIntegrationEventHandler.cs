using DDDMart.Application;
using DDDMart.Ordering.Core.Orders.Repositories;
using Microsoft.Extensions.Logging;

namespace DDDMart.Ordering.Application.IntegrationEvents.Handlers
{
    public class InvoicePaidIntegrationEventHandler : IIntegrationEventHandler<InvoicePaidIntegrationEvent>
    {
        private readonly ILogger<InvoicePaidIntegrationEventHandler> _logger;
        private readonly IOrdersRepository _ordersRepository;

        public InvoicePaidIntegrationEventHandler(ILogger<InvoicePaidIntegrationEventHandler> logger,
            IOrdersRepository ordersRepository)
        {
            _logger = logger;
            _ordersRepository = ordersRepository;
        }

        public async Task HandleAsync(InvoicePaidIntegrationEvent @event)
        {
            _logger.LogInformation("Setting order to paid {order}", @event.OrderId);
            var order = await _ordersRepository.GetByIdAsync(@event.OrderId);
            order.Pay();
            _logger.LogInformation("Dispatching order {order}", @event.OrderId);
            order.Dispatch();
            await _ordersRepository.UnitOfWork.CommitAsync();
        }
    }
}
