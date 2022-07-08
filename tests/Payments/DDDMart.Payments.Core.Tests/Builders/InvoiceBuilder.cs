using DDDMart.Payments.Core.Invoices.Entities;
using DDDMart.Payments.Core.Invoices.ValueObjects;

namespace DDDMart.Payments.Core.Tests.Builders
{
    public class InvoiceBuilder
    {
        private Guid _customerId = Guid.NewGuid();
        private Guid _paymentMethodId = Guid.NewGuid();
        private Guid _orderId = Guid.NewGuid();
        private string _street = "1 Feather Lane";
        private string _city = "Test City";
        private string _state = "New York";
        private string _zipCode = "536354";
        private string _country = "USA";
        private decimal _amount = 100;

        public Invoice Build()
        {
            return Invoice.Generate(_customerId, _orderId, _paymentMethodId, InvoiceAddress.Create(_street, _city, _state, _country, _zipCode), _amount);
        }
    }
}
