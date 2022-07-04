using DDDMart.SharedKernel.Outbox.Entities;

namespace DDDMart.SharedKernel.Outbox.Factories
{
    public interface IIntegrationEventFactory
    {
        IntegrationEvent Create(OutboxIntegrationEvent integrationEvent);
    }
}
