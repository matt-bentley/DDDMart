using DDDMart.SharedKernel;

namespace DDDMart.Ordering.Core.Baskets.DomainEvents
{
    public record BasketCheckedOutDomainEvent(Guid BasketId, Guid CustomerId) : DomainEvent;
}
