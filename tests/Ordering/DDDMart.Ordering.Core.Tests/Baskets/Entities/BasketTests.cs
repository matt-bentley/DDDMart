using DDDMart.Ordering.Core.Baskets.DomainEvents;
using DDDMart.Ordering.Core.Common.ValueObjects;
using DDDMart.Ordering.Core.Tests.Builders;
using DDDMart.SharedKernel.Exceptions;

namespace DDDMart.Ordering.Core.Tests.Baskets.Entities
{
    [TestClass]
    public class BasketTests
    {
        [TestMethod]
        public void GivenBasket_WhenCreate_ThenNotCheckedOut()
        {
            var basket = new BasketBuilder().Build();
            basket.CheckedOut.Should().BeFalse();
        }

        [TestMethod]
        public void GivenBasket_WhenAddSameProduct_ThenAddToBasketItem()
        {
            var basket = new BasketBuilder().Build();
            var sqlProduct = new OrderProduct(Guid.NewGuid(), "SQL Server", 100);
            basket.AddItem(sqlProduct);
            basket.AddItem(sqlProduct);
            basket.Items.Should().HaveCount(1);
            basket.Items.First().Quantity.Should().Be(2);
        }

        [TestMethod]
        public void GivenBasket_WhenRemoveSameProduct_ThenReduceQuantity()
        {
            var basket = new BasketBuilder().Build();
            var sqlProduct = new OrderProduct(Guid.NewGuid(), "SQL Server", 100);
            basket.AddItem(sqlProduct);
            basket.AddItem(sqlProduct);
            basket.RemoveItem(sqlProduct);
            basket.Items.First().Quantity.Should().Be(1);
        }

        [TestMethod]
        public void GivenBasket_WhenRemoveProductFromEmptyBasket_ThenThrow()
        {
            var basket = new BasketBuilder().Build();
            var sqlProduct = new OrderProduct(Guid.NewGuid(), "SQL Server", 100);
            Action action = () => basket.RemoveItem(sqlProduct);
            action.Should().Throw<DomainException>();
        }

        [TestMethod]
        public void GivenBasket_WhenCheckout_ThenSetCheckoutOut()
        {
            var basket = new BasketBuilder().Build();
            var sqlProduct = new OrderProduct(Guid.NewGuid(), "SQL Server", 100);
            basket.AddItem(sqlProduct);
            basket.Checkout();
            basket.CheckedOut.Should().BeTrue();
            basket.DomainEvents.Where(e => e is BasketCheckedOutDomainEvent).Should().HaveCount(1);
        }

        [TestMethod]
        public void GivenBasket_WhenCheckoutEmptyBasket_ThenThrow()
        {
            var basket = new BasketBuilder().Build();
            var sqlProduct = new OrderProduct(Guid.NewGuid(), "SQL Server", 100);
            Action action = () => basket.Checkout();
            action.Should().Throw<DomainException>().WithMessage("Cannot check-out as the basket is empty");
        }

        [TestMethod]
        public void GivenBasket_WhenCheckoutAlreadyCheckedOut_ThenThrow()
        {
            var basket = new BasketBuilder().Build();
            var sqlProduct = new OrderProduct(Guid.NewGuid(), "SQL Server", 100);
            basket.AddItem(sqlProduct);
            basket.Checkout();
            Action action = () => basket.Checkout();
            action.Should().Throw<DomainException>().WithMessage("The basket is already checked-out");
        }
    }
}
