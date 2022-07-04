using DDDMart.Ordering.Core.Orders.ValueObjects;

namespace DDDMart.Ordering.Core.Orders.Factories
{
    public class AddressFactory
    {
        public ShippingAddress CreateShipping(string street, string city, string state, string country, string zipCode)
        {
            var address = new ShippingAddress(street, city, state, country, zipCode);
            address.Validate();
            return address;
        }

        public PaymentAddress CreatePayment(string street, string city, string state, string country, string zipCode)
        {
            var address = new PaymentAddress(street, city, state, country, zipCode);
            address.Validate();
            return address;
        }
    }
}
