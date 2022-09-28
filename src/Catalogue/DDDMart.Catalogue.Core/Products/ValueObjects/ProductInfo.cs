using DDDMart.SharedKernel;
using DDDMart.SharedKernel.Guards;

namespace DDDMart.Catalogue.Core.Products.ValueObjects
{
    public class ProductInfo : ValueObject<ProductInfo>
    {
        private ProductInfo(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public static ProductInfo Create(string name, string description)
        {
            Guard.Against.NullOrEmpty(name, "Name");
            Guard.Against.NullOrEmpty(description, "Description");
            return new ProductInfo(name, description);
        }

        public string Name { get; private set; }
        public string Description { get; private set; }

        protected override int GetValueHashCode()
        {
            return HashCode.Combine(Name);
        }

        protected override bool ValueEquals(ProductInfo other)
        {
            return Name.Equals(other.Name);
        }
    }
}
