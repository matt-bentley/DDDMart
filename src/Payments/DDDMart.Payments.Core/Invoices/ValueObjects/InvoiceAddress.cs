using DDDMart.SharedKernel.ValueObjects;

namespace DDDMart.Payments.Core.Invoices.ValueObjects
{
    public class InvoiceAddress : Address
    {
        protected InvoiceAddress(string street, string city, string state, string country, string zipCode) : base(street, city, state, country, zipCode)
        {
        }

        public static InvoiceAddress Create(string street, string city, string state, string country, string zipCode)
        {
            var address = new InvoiceAddress(street, city, state, country, zipCode);
            address.Validate();
            return address;
        }
    }
}
