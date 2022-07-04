using DDDMart.Infrastructure.Repositories;
using DDDMart.Ordering.Core.Baskets.Entities;
using DDDMart.Ordering.Core.Baskets.Repositories;

namespace DDDMart.Ordering.Infrastructure.Repositories
{
    public class BasketsRepository : Repository<Basket, OrderingContext>, IBasketsRepository
    {
        public BasketsRepository(OrderingContext context) : base(context)
        {
        }
    }
}
