using DDDMart.SharedKernel;

namespace DDDMart.Ordering.Core.Orders.DomainEvents
{
    public record OrderDispatchedDomainEvent(Guid Id, Guid CustomerId, DateTime dispatchedDate) : DomainEvent;
}
