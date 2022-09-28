using DDDMart.SharedKernel;

namespace DDDMart.Catalogue.Core.Reviews.ValueObjects
{
    public class Customer : ValueObject<Customer>
    {
        public Customer(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }

        protected override int GetValueHashCode()
        {
            return Id.GetHashCode();
        }

        protected override bool ValueEquals(Customer other)
        {
            return Id.Equals(other.Id);
        }
    }
}
