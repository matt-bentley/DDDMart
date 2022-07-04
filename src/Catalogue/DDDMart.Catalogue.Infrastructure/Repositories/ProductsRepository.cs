using DDDMart.Catalogue.Core.Entities;
using DDDMart.Catalogue.Core.Repositories;
using DDDMart.Infrastructure.Repositories;

namespace DDDMart.Catalogue.Infrastructure.Repositories
{
    public class ProductsRepository : Repository<Product, CatalogueContext>, IProductsRepository
    {
        public ProductsRepository(CatalogueContext context) : base(context)
        {
        }
    }
}
