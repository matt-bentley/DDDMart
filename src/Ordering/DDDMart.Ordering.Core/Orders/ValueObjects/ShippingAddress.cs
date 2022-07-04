using DDDMart.SharedKernel.ValueObjects;

namespace DDDMart.Ordering.Core.Orders.ValueObjects
{
    public class ShippingAddress : Address
    {
        internal ShippingAddress(string street, string city, string state, string country, string zipCode) : base(street, city, state, country, zipCode)
        {
        }
    }
}
