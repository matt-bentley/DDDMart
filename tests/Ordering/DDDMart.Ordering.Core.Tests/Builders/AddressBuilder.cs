using DDDMart.Ordering.Core.Orders.Factories;
using DDDMart.Ordering.Core.Orders.ValueObjects;

namespace DDDMart.Ordering.Core.Tests.Builders
{
    public class AddressBuilder
    {
        private readonly AddressFactory _factory = new AddressFactory();
        private string _street = "1 Feather Lane";
        private string _city = "Test City";
        private string _state = "New York";
        private string _zipCode = "536354";
        private string _country = "USA";

        public ShippingAddress BuildShipping()
        {
            return _factory.CreateShipping(_street, _city, _state, _country, _zipCode);
        }

        public PaymentAddress BuildPayment()
        {
            return _factory.CreatePayment(_street, _city, _state, _country, _zipCode);
        }
    }
}
