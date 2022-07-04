using DDDMart.SharedKernel;

namespace DDDMart.Application
{
    public interface IIntegrationEventHandler<T> where T : IntegrationEvent
    {
        Task HandleAsync(T @event);
    }
}
