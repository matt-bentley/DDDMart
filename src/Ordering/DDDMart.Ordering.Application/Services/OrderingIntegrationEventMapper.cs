using DDDMart.Application;
using DDDMart.Ordering.Application.IntegrationEvents;
using DDDMart.Ordering.Core;
using DDDMart.Ordering.Core.Orders.DomainEvents;
using DDDMart.SharedKernel;

namespace DDDMart.Ordering.Application.Services
{
    public class OrderingIntegrationEventMapper : IntegrationEventMapper, IOrderingIntegrationEventMapper
    {
        protected override IntegrationEvent MapDomainEvent<T>(T domainEvent)
        {
            return domainEvent switch
            {
                OrderSubmittedDomainEvent @event => new OrderSubmittedIntegrationEvent(@event.Id, @event.CustomerId, @event.PaymentMethodId, @event.PaymentAddress.Street, @event.PaymentAddress.City, @event.PaymentAddress.State, @event.PaymentAddress.Country, @event.PaymentAddress.ZipCode, @event.TotalPrice),
                { } => null
            };
        }
    }
}
