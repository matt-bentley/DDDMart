using DDDMart.SharedKernel;

namespace DDDMart.Ordering.Application.IntegrationEvents
{
    public class InvoicePaidIntegrationEvent : IntegrationEvent
    {
        public InvoicePaidIntegrationEvent(Guid id, Guid customerId, Guid orderId, DateTime paidDate) :base(id)
        {
            CustomerId = customerId;
            OrderId = orderId;
            PaidDate = paidDate;
        }

        public readonly Guid CustomerId;
        public readonly Guid OrderId;
        public readonly DateTime PaidDate;
    }
}
