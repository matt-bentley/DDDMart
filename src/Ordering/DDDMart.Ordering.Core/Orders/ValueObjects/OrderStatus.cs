
namespace DDDMart.Ordering.Core.Orders.ValueObjects
{
    public enum OrderStatus
    {
        Draft = 0,
        ShippingAddressConfirmed = 100,
        PaymentMethodConfirmed = 120,
        Submitted = 150,
        Paid = 200,
        Dispatched = 300,
        Delivered = 400,
        Cancelled = 900
    }
}
