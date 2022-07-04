using DDDMart.Application.Factories;
using DDDMart.SharedKernel;
using DDDMart.SharedKernel.Outbox.Entities;
using DDDMart.SharedKernel.Outbox.Factories;
using DDDMart.SharedKernel.Outbox.Services;
using Newtonsoft.Json;

namespace DDDMart.Application
{
    public abstract class IntegrationEventMapper : IIntegrationEventMapper
    {
        public IntegrationEventMapper()
        {
            Factory = new IntegrationEventFactory(this.GetType().Assembly);
        }

        public IIntegrationEventFactory Factory { get; }

        public List<OutboxIntegrationEvent> Map(IEnumerable<DomainEvent> domainEvents)
        {
            var integrationEvents = domainEvents.Select(e => MapDomainEvent(e))
                                                .Where(e => e != null)
                                                .ToList();

            return integrationEvents.Select(e => MapIntegrationEvent(e)).ToList();
        }

        protected abstract IntegrationEvent MapDomainEvent<T>(T domainEvent) where T : DomainEvent;

        private OutboxIntegrationEvent MapIntegrationEvent(IntegrationEvent integrationEvent)
        {
            var json = JsonConvert.SerializeObject(integrationEvent);
            return new OutboxIntegrationEvent(integrationEvent.GetType().Name, json);
        }
    }
}
