using DDDMart.SharedKernel;

namespace DDDMart.Ordering.Core.Orders.DomainEvents
{
    public record OrderCancelledDomainEvent(Guid Id, Guid CustomerId) : DomainEvent;
}
