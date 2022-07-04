using DDDMart.Ordering.Core.Common.ValueObjects;
using DDDMart.Ordering.Core.Orders.DomainEvents;
using DDDMart.Ordering.Core.Orders.Entities;
using DDDMart.Ordering.Core.Orders.ValueObjects;
using DDDMart.Ordering.Core.Tests.Builders;
using DDDMart.SharedKernel.Exceptions;

namespace DDDMart.Ordering.Core.Tests.Orders.Entities
{
    [TestClass]
    public class OrderTests
    {
        [TestMethod]
        public void GivenOrder_WhenCreate_ThenDraftStatus()
        {
            var order = new OrderBuilder().Build();
            order.Status.Should().Be(OrderStatus.Draft);
            order.DispatchedDate.Should().BeNull();
            order.DeliveredDate.Should().BeNull();
        }

        [TestMethod]
        public void GivenOrder_WhenCancel_ThenSetCancelled()
        {
            var order = new OrderBuilder().Build();
            order.Cancel();
            order.Status.Should().Be(OrderStatus.Cancelled);
            order.DomainEvents.Where(e => e is OrderCancelledDomainEvent).Should().HaveCount(1);
        }

        [TestMethod]
        public void GivenOrder_WhenCreateWithBasketItems_ThenAddItems()
        {
            var basket = new BasketBuilder().Build();
            var sqlProduct = new OrderProduct(Guid.NewGuid(), "SQL Server", 100);
            basket.AddItem(sqlProduct);
            basket.AddItem(sqlProduct);
            var order = Order.FromBasket(basket);
            order.Items.Should().HaveCount(1);
            order.Items.First().Quantity.Should().Be(2);
        }

        [TestMethod]
        public void GivenOrder_WhenUpdateShippingAddress_ThenUpdateStatusAndAddress()
        {
            var order = new OrderBuilder().Build();
            order.UpdateShippingAddress(new AddressBuilder().BuildShipping());
            order.Status.Should().Be(OrderStatus.ShippingAddressConfirmed);
            order.ShippingAddress.Should().NotBeNull();
        }

        [TestMethod]
        public void GivenOrder_WhenUpdatePaymentMethod_ThenUpdateStatusAndAddress()
        {
            var order = new OrderBuilder().Build();
            order.UpdateShippingAddress(new AddressBuilder().BuildShipping());
            order.UpdatePaymentMethod(Guid.NewGuid(), new AddressBuilder().BuildPayment());
            order.Status.Should().Be(OrderStatus.PaymentMethodConfirmed);
            order.PaymentAddress.Should().NotBeNull();
        }

        [TestMethod]
        public void GivenOrder_WhenUpdatePaymentMethodBeforeShippingAddress_ThenThrow()
        {
            var order = new OrderBuilder().Build();
            Action action = () => order.UpdatePaymentMethod(Guid.NewGuid(), new AddressBuilder().BuildPayment());
            action.Should().Throw<DomainException>().WithMessage("Shipping Address must be selected before Payment Method");
        }

        [TestMethod]
        public void GivenOrder_WhenSubmit_ThenUpdateStatus()
        {
            var order = new OrderBuilder().Build();
            order.UpdateShippingAddress(new AddressBuilder().BuildShipping());
            order.UpdatePaymentMethod(Guid.NewGuid(), new AddressBuilder().BuildPayment());
            order.Submit();
            order.Status.Should().Be(OrderStatus.Submitted);
            order.DomainEvents.Where(e => e is OrderSubmittedDomainEvent).Should().HaveCount(1);
        }

        [TestMethod]
        public void GivenOrder_WhenSubmitCancelled_ThenThrow()
        {
            var order = new OrderBuilder().Build();
            order.UpdateShippingAddress(new AddressBuilder().BuildShipping());
            order.UpdatePaymentMethod(Guid.NewGuid(), new AddressBuilder().BuildPayment());
            order.Cancel();
            Action action = () => order.Submit();
            action.Should().Throw<DomainException>().WithMessage("Order has been cancelled.");
        }

        [TestMethod]
        public void GivenOrder_WhenSubmitBeforeComplete_ThenThrow()
        {
            var order = new OrderBuilder().Build();
            Action action = () => order.Submit();
            action.Should().Throw<DomainException>();
        }
    }
}
