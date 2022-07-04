using MediatR;

namespace DDDMart.SharedKernel
{
    public interface IDomainEventHandler<T> where T : DomainEvent
    {
        Task HandleAsync(T @event);
    }

    public abstract class DomainEventHandler<T> : IDomainEventHandler<T>, INotificationHandler<T> where T : DomainEvent
    {
        public async Task Handle(T notification, CancellationToken cancellationToken)
        {
            await HandleAsync(notification);
        }

        public abstract Task HandleAsync(T @event);
    }
}
