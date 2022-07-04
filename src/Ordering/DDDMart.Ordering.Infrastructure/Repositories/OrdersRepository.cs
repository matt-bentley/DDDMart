using DDDMart.Infrastructure.Repositories;
using DDDMart.Ordering.Core.Orders.Entities;
using DDDMart.Ordering.Core.Orders.Repositories;

namespace DDDMart.Ordering.Infrastructure.Repositories
{
    public class OrdersRepository : Repository<Order, OrderingContext>, IOrdersRepository
    {
        public OrdersRepository(OrderingContext context) : base(context)
        {
        }
    }
}
