using DDDMart.Application;
using DDDMart.Payments.Application.IntegrationEvents;
using DDDMart.Payments.Core;
using DDDMart.Payments.Core.Invoices.DomainEvents;
using DDDMart.SharedKernel;

namespace DDDMart.Payments.Application.Services
{
    public class PaymentsIntegrationEventMapper : IntegrationEventMapper, IPaymentsIntegrationEventMapper
    {
        protected override IntegrationEvent MapDomainEvent<T>(T domainEvent)
        {
            return domainEvent switch
            {
                InvoicePaidDomainEvent @event => new InvoicePaidIntegrationEvent(@event.Id, @event.CustomerId, @event.OrderId, @event.PaidDate),
                { } => null
            };
        }
    }
}
