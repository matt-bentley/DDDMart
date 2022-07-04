using DDDMart.SharedKernel;
using DDDMart.SharedKernel.Outbox.Entities;
using DDDMart.SharedKernel.Outbox.Factories;
using Newtonsoft.Json;
using System.Reflection;

namespace DDDMart.Application.Factories
{
    internal class IntegrationEventFactory : IIntegrationEventFactory
    {
        private readonly Dictionary<string, Type> _eventTypes;

        public IntegrationEventFactory(Assembly integrationEventAssembly)
        {
            var baseEventType = typeof(IntegrationEvent);
            _eventTypes = integrationEventAssembly
                            .GetTypes()
                            .Where(e => baseEventType.IsAssignableFrom(e))
                            .ToDictionary(e => e.Name);
        }

        public IntegrationEvent Create(OutboxIntegrationEvent integrationEvent)
        {
            var eventType = _eventTypes[integrationEvent.EventName];
            var @event = JsonConvert.DeserializeObject(integrationEvent.Data, eventType);
            return (IntegrationEvent)@event;
        }
    }
}
