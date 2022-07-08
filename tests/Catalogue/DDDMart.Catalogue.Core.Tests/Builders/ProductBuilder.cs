using DDDMart.Catalogue.Core.Entities;
using DDDMart.Catalogue.Core.ValueObjects;
using System.Reflection;

namespace DDDMart.Catalogue.Core.Tests.Builders
{
    public class ProductBuilder
    {
        private Guid? _id;
        private string _name = "SQL Server";
        private string _description = "SQL Server relational database";
        private string _sicCode = "1234";
        private decimal _price = 100;

        public Product Build()
        {
            var product = Product.Create(ProductInfo.Create(_name, _description), _sicCode, _price, Picture.Create("Test", "https://test.com"));
            if (_id.HasValue)
            {
                product.GetType().GetProperty(nameof(product.Id), BindingFlags.Public | BindingFlags.Instance).SetValue(product, _id.Value, null);
            }
            return product;
        }

        public ProductBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public ProductBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public ProductBuilder WithPrice(decimal price)
        {
            _price = price;
            return this;
        }
    }
}
