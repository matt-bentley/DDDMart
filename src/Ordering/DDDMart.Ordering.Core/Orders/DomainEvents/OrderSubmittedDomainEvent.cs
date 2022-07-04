using DDDMart.Ordering.Core.Orders.ValueObjects;
using DDDMart.SharedKernel;

namespace DDDMart.Ordering.Core.Orders.DomainEvents
{
    public record OrderSubmittedDomainEvent(Guid Id, Guid CustomerId, Guid PaymentMethodId, PaymentAddress PaymentAddress, decimal TotalPrice) : DomainEvent;
}
