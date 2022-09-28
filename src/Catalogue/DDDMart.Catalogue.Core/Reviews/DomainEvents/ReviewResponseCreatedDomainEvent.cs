using DDDMart.Catalogue.Core.Reviews.ValueObjects;
using DDDMart.SharedKernel;

namespace DDDMart.Catalogue.Core.Reviews.DomainEvents
{
    public record ReviewResponseCreatedDomainEvent(Guid ReviewId, Guid ResponseId, Customer Customer, Comment comment) : DomainEvent;
}
