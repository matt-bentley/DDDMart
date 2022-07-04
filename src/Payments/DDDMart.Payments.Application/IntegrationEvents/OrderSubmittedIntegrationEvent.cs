using DDDMart.SharedKernel;

namespace DDDMart.Payments.Application.IntegrationEvents
{
    public class OrderSubmittedIntegrationEvent : IntegrationEvent
    {
        public OrderSubmittedIntegrationEvent(Guid id, Guid customerId, Guid paymentMethodId, string paymentStreet, 
            string paymentCity, string paymentState, string paymentCountry, string paymentZipCode, decimal totalPrice) : base(id)
        {
            CustomerId = customerId;
            PaymentMethodId = paymentMethodId;
            PaymentStreet = paymentStreet;
            PaymentCity = paymentCity;
            PaymentState = paymentState;
            PaymentCountry = paymentCountry;
            PaymentZipCode = paymentZipCode;
            TotalPrice = totalPrice;
        }

        public readonly Guid CustomerId;
        public readonly Guid PaymentMethodId;
        public readonly string PaymentStreet;
        public readonly string PaymentCity;
        public readonly string PaymentState;
        public readonly string PaymentCountry;
        public readonly string PaymentZipCode;
        public readonly decimal TotalPrice;
    }
}
