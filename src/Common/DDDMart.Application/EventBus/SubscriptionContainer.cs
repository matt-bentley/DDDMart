using DDDMart.SharedKernel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DDDMart.Application.EventBus
{
    public class SubscriptionContainer : ISubscriptionContainer
    {
        private readonly object _lock = new object();
        private Dictionary<Type, List<HandlerSubscription>> _subscriptions = new Dictionary<Type, List<HandlerSubscription>>();
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SubscriptionContainer> _logger;
        private readonly Dictionary<string, Type> _eventTypes = new Dictionary<string, Type>();

        public SubscriptionContainer(IServiceProvider serviceProvider,
            ILogger<SubscriptionContainer> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public IReadOnlyList<Type> EventTypes => _eventTypes.Values.ToList();

        public Type GetEventType(string eventName)
        {
            return _eventTypes[eventName];
        }

        public void Register<TEvent, THandler>()
            where TEvent : IntegrationEvent
            where THandler : IIntegrationEventHandler<TEvent>
        {
            var eventType = typeof(TEvent);
            var handlerType = typeof(THandler);
            lock (_lock)
            {
                _eventTypes.Add(eventType.Name, eventType);
                if (!_subscriptions.TryGetValue(eventType, out List<HandlerSubscription> handlers))
                {
                    handlers = new List<HandlerSubscription>();
                    _subscriptions[eventType] = handlers;
                }
                handlers.Add(new HandlerSubscription(handlerType));
            }
        }

        public async Task HandleAsync<T>(T @event) where T : IntegrationEvent
        {
            var eventType = @event.GetType();
            if (!_subscriptions.TryGetValue(eventType, out List<HandlerSubscription> handlerSubscriptions))
            {
                throw new IndexOutOfRangeException($"No handlers registered for {eventType.Name}");
            }

            foreach (var handlerSubscription in handlerSubscriptions)
            {
                _logger.LogInformation("Processing {handler}", handlerSubscription.Type.Name);
                using (var scope = _serviceProvider.CreateScope())
                {
                    try
                    {
                        var handler = scope.ServiceProvider.GetRequiredService(handlerSubscription.Type);
                        await (Task)handlerSubscription.MethodInfo.Invoke(handler, new[] { @event });
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error handling integration event - {ex}", ex.ToString());
                    }
                }
            }
        }
    }
}
