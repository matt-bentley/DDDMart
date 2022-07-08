using DDDMart.SharedKernel;

namespace DDDMart.Ordering.Application.IntegrationEvents
{
    public class OrderCancelledIntegrationEvent : IntegrationEvent
    {
        public OrderCancelledIntegrationEvent(Guid id, Guid customerId) : base(id)
        {
            CustomerId = customerId;
        }

        public readonly Guid CustomerId;
    }
}
