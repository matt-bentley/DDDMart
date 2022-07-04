using DDDMart.Ordering.Core.Baskets.Entities;
using DDDMart.Ordering.Core.Orders.DomainEvents;
using DDDMart.Ordering.Core.Orders.ValueObjects;
using DDDMart.SharedKernel;
using DDDMart.SharedKernel.Exceptions;

namespace DDDMart.Ordering.Core.Orders.Entities
{
    public class Order : AggregateRoot
    {
        private Order(Guid customerId, Guid basketId, OrderStatus orderStatus)
        {
            CustomerId = customerId;
            BasketId = basketId;
            Status = orderStatus;
        }

        private Order()
        {

        }

        public static Order FromBasket(Basket basket)
        {
            var order = new Order(basket.CustomerId, basket.Id, OrderStatus.Draft);
            foreach(var item in basket.Items.Where(e => !e.Empty))
            {
                order.AddItemFromBasket(item);
            }
            return order;
        }

        public Guid CustomerId { get; private set; }
        public Guid BasketId { get; private set; }
        public OrderStatus Status { get; private set; }
        public ShippingAddress ShippingAddress { get; private set; }
        public Guid PaymentMethodId { get; private set; }
        public PaymentAddress PaymentAddress { get; private set; }
        public decimal TotalPrice => _items.Sum(e => e.TotalPrice);
        public bool Submitted => Status >= OrderStatus.Submitted;
        public DateTime? DispatchedDate { get; private set; }
        public bool Dispatched => DispatchedDate.HasValue;
        public DateTime? DeliveredDate { get; private set; }
        public bool Delivered => DeliveredDate.HasValue;
        public bool Cancelled => Status == OrderStatus.Cancelled;

        private readonly List<OrderItem> _items = new List<OrderItem>();
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        private void AddItemFromBasket(BasketItem item)
        {
            _items.Add(OrderItem.FromBasketItem(item));
        }

        public void UpdateShippingAddress(ShippingAddress shippingAddress)
        {
            if(Status >= OrderStatus.Dispatched)
            {
                throw new DomainException("Cannot update Shipping Address, order has already been dispatched");
            }
            if(Status < OrderStatus.ShippingAddressConfirmed)
            {
                Status = OrderStatus.ShippingAddressConfirmed;
            }
            ShippingAddress = shippingAddress;
        }

        public void UpdatePaymentMethod(Guid paymentMethodId, PaymentAddress paymentAddress)
        {
            if(Status < OrderStatus.ShippingAddressConfirmed)
            {
                throw new DomainException("Shipping Address must be selected before Payment Method");
            }
            if (Status >= OrderStatus.Paid)
            {
                throw new DomainException("Cannot update Payment Method, payment has already been made");
            }
            if (Status < OrderStatus.PaymentMethodConfirmed)
            {
                Status = OrderStatus.PaymentMethodConfirmed;
            }
            PaymentMethodId = paymentMethodId;
            PaymentAddress = paymentAddress;
        }

        public void Submit()
        {
            CheckIfCancelled();
            if (Submitted)
            {
                throw new DomainException("Order has already been submitted");
            }
            if(Status != OrderStatus.PaymentMethodConfirmed)
            {
                throw new DomainException("Payment Method and Delivery Address must be selected before submitting the order");
            }
            Status = OrderStatus.Submitted;
            AddDomainEvent(new OrderSubmittedDomainEvent(Id, CustomerId, PaymentMethodId, PaymentAddress, TotalPrice));
        }

        public void Pay()
        {
            CheckIfCancelled();
            if (!Submitted)
            {
                throw new DomainException("Order has not been submitted");
            }
            Status = OrderStatus.Paid;
        }

        public void Dispatch()
        {
            CheckIfCancelled();
            if (Status != OrderStatus.Paid)
            {
                throw new DomainException("Order must be paid before dispatching");
            }
            DispatchedDate = DateTime.UtcNow;
            Status = OrderStatus.Dispatched;
            AddDomainEvent(new OrderDispatchedDomainEvent(Id, CustomerId, DispatchedDate.Value));
        }

        public void Deliver()
        {
            if (!Dispatched)
            {
                throw new DomainException("Order has not been dispatched yet");
            }
            DeliveredDate = DateTime.UtcNow;
            Status= OrderStatus.Delivered;
        }

        public void Cancel()
        {
            Status = OrderStatus.Cancelled;
            AddDomainEvent(new OrderCancelledDomainEvent(Id, CustomerId));
        }

        private void CheckIfCancelled()
        {
            if (Cancelled)
            {
                throw new DomainException("Order has been cancelled.");
            }
        }
    }
}
