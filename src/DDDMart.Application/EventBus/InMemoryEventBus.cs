using DDDMart.SharedKernel;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Channels;

namespace DDDMart.Application.EventBus
{
    public class InMemoryEventBus : IEventBus
    {
        private readonly ILogger<InMemoryEventBus> _logger;
        private readonly Channel<Message> _channel;
        private readonly object _lock = new object();
        private bool _isProcessing = false;
        private Task _processingTask;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly ISubscriptionContainer _subscriptionContainer;

        public InMemoryEventBus(ILogger<InMemoryEventBus> logger,
            ISubscriptionContainer subscriptionContainer)
        {
            _logger = logger;
            _subscriptionContainer = subscriptionContainer;
            _channel = Channel.CreateUnbounded<Message>();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task PublishAsync(IntegrationEvent @event)
        {
            _logger.LogInformation("Publishing {type}", @event.GetType().Name);
            var message = GetEventMessage(@event);
            await _channel.Writer.WriteAsync(message, CancellationToken.None);
        }

        private Message GetEventMessage(IntegrationEvent @event)
        {
            var body = JsonConvert.SerializeObject(@event);
            return new Message(body, @event.GetType().Name, DateTime.UtcNow);
        }

        public void Subscribe<TEvent, THandler>()
            where TEvent : IntegrationEvent
            where THandler : IIntegrationEventHandler<TEvent>
        {
            _logger.LogInformation("Subscribing to {type} with {handler}", typeof(TEvent).Name, typeof(THandler).Name);
            _subscriptionContainer.Register<TEvent, THandler>();
            TryStartProcessing();
        }

        private void TryStartProcessing()
        {
            bool startProcessing = false;
            lock (_lock)
            {
                if (!_isProcessing)
                {
                    _isProcessing = true;
                    startProcessing = true;
                }
            }
            if (startProcessing)
            {
                StartProcessing(_cancellationTokenSource.Token);
            }
        }

        private void StartProcessing(CancellationToken cancellationToken)
        {
            _processingTask = Task.Factory.StartNew(async () =>
            {
                _logger.LogInformation("Listening for events");
                while (await _channel.Reader.WaitToReadAsync(cancellationToken))
                {
                    while (_channel.Reader.TryRead(out Message message))
                    {
                        _logger.LogInformation("Processing {type}", message.EventType);
                        var @event = GetIntegrationEvent(message);
                        await _subscriptionContainer.HandleAsync(@event);
                        _logger.LogInformation("Completed processing {type}", @event.GetType().Name);
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        private IntegrationEvent GetIntegrationEvent(Message message)
        {
            var eventType = _subscriptionContainer.GetEventType(message.EventType);
            var @event = (IntegrationEvent)JsonConvert.DeserializeObject(message.Body, eventType);
            return @event;
        }

        public async ValueTask DisposeAsync()
        {
            if (_isProcessing)
            {
                _logger.LogInformation("Stopping event bus");
                _cancellationTokenSource.Cancel();
                await _processingTask;
                _isProcessing = false;
            }
        }
    }
}
