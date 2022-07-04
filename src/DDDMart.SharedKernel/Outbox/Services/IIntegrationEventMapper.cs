using DDDMart.SharedKernel.Outbox.Entities;
using DDDMart.SharedKernel.Outbox.Factories;

namespace DDDMart.SharedKernel.Outbox.Services
{
    public interface IIntegrationEventMapper
    {
        public IIntegrationEventFactory Factory { get; }
        List<OutboxIntegrationEvent> Map(IEnumerable<DomainEvent> domainEvents);
    }
}
