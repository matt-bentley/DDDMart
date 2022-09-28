using DDDMart.Catalogue.Core.Reviews.ValueObjects;
using DDDMart.SharedKernel;

namespace DDDMart.Catalogue.Core.Reviews.DomainEvents
{
    public record ReviewCreatedDomainEvent(Guid Id, Customer Customer, Guid OrderId, Rating rating, Comment comment) : DomainEvent;
}
