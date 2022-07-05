using DDDMart.Catalogue.Core.Entities;
using DDDMart.Catalogue.Core.ValueObjects;

namespace DDDMart.Catalogue.Core.Tests.Builders
{
    public class ProductBuilder
    {
        private string _name = "SQL Server";
        private string _description = "SQL Server relational database";
        private string _sicCode = "1234";
        private decimal _price = 100;

        public Product Build()
        {
            return Product.Create(ProductInfo.Create(_name, _description), _sicCode, _price, Picture.Create("Test", "https://test.com"));
        }
    }
}
