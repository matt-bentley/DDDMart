using DDDMart.Ordering.Core.Baskets.DomainEvents;
using DDDMart.Ordering.Core.Baskets.Repositories;
using DDDMart.Ordering.Core.Orders.DomainEventHandlers;
using DDDMart.Ordering.Core.Orders.Entities;
using DDDMart.Ordering.Core.Orders.Repositories;
using DDDMart.Ordering.Core.Tests.Builders;
using Microsoft.Extensions.Logging;

namespace DDDMart.Ordering.Core.Tests.DomainEventHandlers
{
    [TestClass]
    public class BasketCheckedOutDomainEventHandlerTests
    {
        private readonly BasketCheckedOutDomainEventHandler _handler;
        private readonly Mock<IBasketsRepository> _basketsRepository = new Mock<IBasketsRepository>();
        private readonly Mock<IOrdersRepository> _ordersRepository = new Mock<IOrdersRepository>();

        public BasketCheckedOutDomainEventHandlerTests()
        {
            _handler = new BasketCheckedOutDomainEventHandler(_basketsRepository.Object, _ordersRepository.Object, Mock.Of<ILogger<BasketCheckedOutDomainEventHandler>>());
        }

        [TestMethod]
        public async Task GivenBasketCheckedOutDomainEvent_WhenHandle_ThenCreateOrder()
        {
            var basket = new BasketBuilder().Build();
            _basketsRepository.Setup(e => e.GetByIdAsync(basket.Id)).ReturnsAsync(basket);

            await _handler.HandleAsync(new BasketCheckedOutDomainEvent(basket.Id, basket.CustomerId));

            _ordersRepository.Verify(e => e.InsertAsync(It.Is<Order>(order => order.BasketId == basket.Id && order.CustomerId == basket.CustomerId)), Times.Once);
        }
    }
}
