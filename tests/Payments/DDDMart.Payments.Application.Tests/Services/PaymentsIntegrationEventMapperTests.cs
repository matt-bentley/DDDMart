using DDDMart.Ordering.Core.Baskets.DomainEvents;
using DDDMart.Payments.Application.IntegrationEvents;
using DDDMart.Payments.Application.Services;
using DDDMart.Payments.Core;
using DDDMart.Payments.Core.Invoices.DomainEvents;
using DDDMart.SharedKernel;

namespace DDDMart.Payments.Application.Tests.Services
{
    [TestClass]
    public class PaymentsIntegrationEventMapperTests
    {
        private readonly IPaymentsIntegrationEventMapper _eventMapper = new PaymentsIntegrationEventMapper();

        [TestMethod]
        public void GivenPaymentsIntegrationEventMapper_WhenMapMappedDomainEvent_ThenCreateIntegrationEvent()
        {
            var integrationEvents = _eventMapper.Map(new List<DomainEvent>() { new InvoicePaidDomainEvent(Guid.NewGuid(), DateTime.UtcNow, Guid.NewGuid(), Guid.NewGuid()) });
            integrationEvents.Should().HaveCount(1);
            integrationEvents.First().EventName.Should().Be(nameof(InvoicePaidIntegrationEvent));
        }

        [TestMethod]
        public void GivenPaymentsIntegrationEventMapper_WhenMapNonMappedDomainEvent_ThenMapEmpty()
        {
            var integrationEvents = _eventMapper.Map(new List<DomainEvent>() { new BasketCheckedOutDomainEvent(Guid.NewGuid(), Guid.NewGuid()) });
            integrationEvents.Should().HaveCount(0);
        }
    }
}
