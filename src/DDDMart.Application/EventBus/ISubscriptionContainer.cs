using DDDMart.SharedKernel;

namespace DDDMart.Application.EventBus
{
    public interface ISubscriptionContainer
    {
        IReadOnlyList<Type> EventTypes { get; }
        Type GetEventType(string eventName);
        void Register<TEvent, THandler>()
            where TEvent : IntegrationEvent
            where THandler : IIntegrationEventHandler<TEvent>;
        Task HandleAsync<T>(T @event) where T : IntegrationEvent;
    }
}
