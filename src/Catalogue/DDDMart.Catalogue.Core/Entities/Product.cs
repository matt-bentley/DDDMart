using DDDMart.Catalogue.Core.ValueObjects;
using DDDMart.SharedKernel;
using DDDMart.SharedKernel.Guards;

namespace DDDMart.Catalogue.Core.Entities
{
    public class Product : AggregateRoot
    {
        private Product(ProductInfo info, string sicCode, decimal price, Picture picture)
        {
            Info = info;
            SicCode = sicCode;
            Price = price;
            Picture = picture;
        }

        private Product()
        {

        }

        public static Product Create(ProductInfo info, string sicCode, decimal price, Picture picture)
        {
            Guard.Against.LessThanZero(price, "Price");
            Guard.Against.NullOrEmpty(sicCode, "Sic Code");
            return new Product(info, sicCode, price, picture);
        }

        public ProductInfo Info { get; private set; }
        public string SicCode { get; private set; }
        public decimal Price { get; private set; }
        public Picture Picture { get; private set; }
    }
}
