using DDDMart.Ordering.Core.Common.ValueObjects;
using DDDMart.Ordering.Core.Orders.Entities;

namespace DDDMart.Ordering.Core.Tests.Builders
{
    public class OrderBuilder
    {
        private Guid _customerId = Guid.NewGuid();

        public Order Build()
        {
            var basket = new BasketBuilder()
                            .WithCustomerId(_customerId)
                            .Build();
            var sqlProduct = new OrderProduct(Guid.NewGuid(), "SQL Server", 100);
            basket.AddItem(sqlProduct);
            return Order.FromBasket(basket);
        }
    }
}
