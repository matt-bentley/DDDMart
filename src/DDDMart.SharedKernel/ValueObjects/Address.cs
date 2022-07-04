using DDDMart.SharedKernel.Guards;

namespace DDDMart.SharedKernel.ValueObjects
{
    public class Address : ValueObject<Address>
    {
        protected Address(string street, string city, string state, string country, string zipCode)
        {
            Street = street;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipCode;
        }

        public string Street { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string Country { get; private set; }
        public string ZipCode { get; private set; }

        public virtual void Validate()
        {
            Guard.Against.NullOrEmpty(Street, "Street");
            Guard.Against.NullOrEmpty(City, "City");
            Guard.Against.NullOrEmpty(State, "State");
            Guard.Against.NullOrEmpty(Country, "Country");
            Guard.Against.NullOrEmpty(ZipCode, "Zip Code");
        }

        protected override int GetValueHashCode()
        {
            return HashCode.Combine(Street, City, State, Country, ZipCode);
        }

        protected override bool ValueEquals(Address other)
        {
            return Street.Equals(other.Street)
                && City.Equals(other.City)
                && State.Equals(other.State)
                && Country.Equals(other.Country)
                && ZipCode.Equals(other.ZipCode);
        }
    }
}
