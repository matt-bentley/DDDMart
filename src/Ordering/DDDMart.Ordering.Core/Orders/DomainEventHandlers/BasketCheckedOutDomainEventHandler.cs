using DDDMart.Ordering.Core.Baskets.DomainEvents;
using DDDMart.Ordering.Core.Baskets.Repositories;
using DDDMart.Ordering.Core.Orders.Entities;
using DDDMart.Ordering.Core.Orders.Repositories;
using DDDMart.SharedKernel;
using Microsoft.Extensions.Logging;

namespace DDDMart.Ordering.Core.Orders.DomainEventHandlers
{
    public class BasketCheckedOutDomainEventHandler : DomainEventHandler<BasketCheckedOutDomainEvent>
    {
        private readonly IBasketsRepository _basketsRepository;
        private readonly IOrdersRepository _ordersRepository;
        private readonly ILogger<BasketCheckedOutDomainEventHandler> _logger;

        public BasketCheckedOutDomainEventHandler(IBasketsRepository basketsRepository,
            IOrdersRepository ordersRepository,
            ILogger<BasketCheckedOutDomainEventHandler> logger)
        {
            _basketsRepository = basketsRepository;
            _ordersRepository = ordersRepository;
            _logger = logger;
        }

        public override async Task HandleAsync(BasketCheckedOutDomainEvent @event)
        {
            _logger.LogInformation("Creating order from basket {id}", @event.BasketId);
            var basket = await _basketsRepository.GetByIdAsync(@event.BasketId);
            var order = Order.FromBasket(basket);
            await _ordersRepository.InsertAsync(order);
            _logger.LogInformation("Created order {id}", order.Id);
        }
    }
}
