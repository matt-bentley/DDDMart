using DDDMart.Payments.Application.IntegrationEvents;
using DDDMart.Payments.Application.IntegrationEvents.Handlers;
using DDDMart.Payments.Core.Invoices.Entities;
using DDDMart.Payments.Core.Invoices.Repositories;
using DDDMart.SharedKernel;
using Microsoft.Extensions.Logging;

namespace DDDMart.Payments.Application.Tests.IntegrationEventHandlers
{
    [TestClass]
    public class OrderSubmittedIntegrationEventHandlerTests
    {
        private readonly OrderSubmittedIntegrationEventHandler _handler;
        private readonly Mock<IInvoicesRepository> _invoicesRepository = new Mock<IInvoicesRepository>();

        public OrderSubmittedIntegrationEventHandlerTests()
        {
            _invoicesRepository.Setup(e => e.UnitOfWork).Returns(Mock.Of<IUnitOfWork>());
            _handler = new OrderSubmittedIntegrationEventHandler(Mock.Of<ILogger<OrderSubmittedIntegrationEventHandler>>(), _invoicesRepository.Object);
        }

        [TestMethod]
        public async Task GivenOrderSubmittedIntegrationEventHandler_WhenHandler_ThenGenerateInvoice()
        {
            var @event = new OrderSubmittedIntegrationEvent(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Street", "City", "State", "Country", "4324324", 100);

            await _handler.HandleAsync(@event);

            _invoicesRepository.Verify(e => e.InsertAsync(It.Is<Invoice>(invoice => invoice.OrderId == @event.Id)), Times.Once);
        }
    }
}
