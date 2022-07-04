using DDDMart.Ordering.Core.Baskets.DomainEvents;
using DDDMart.Ordering.Core.Common.ValueObjects;
using DDDMart.SharedKernel;
using DDDMart.SharedKernel.Exceptions;

namespace DDDMart.Ordering.Core.Baskets.Entities
{
    public class Basket : AggregateRoot
    {
        public Basket(Guid customerId)
        {
            CustomerId = customerId;
            CheckedOut = false;
        }

        public Guid CustomerId { get; private set; }
        public bool CheckedOut { get; private set; }

        private readonly List<BasketItem> _items = new List<BasketItem>();
        public IReadOnlyCollection<BasketItem> Items => _items.AsReadOnly();

        public bool Empty => !_items.Any() || _items.All(e => e.Empty);

        public void AddItem(OrderProduct product)
        {
            var item = GetItem(product);
            if(item == null)
            {
                _items.Add(BasketItem.Create(product));
            }
            else
            {
                item.Add();
            }
        }

        public void RemoveItem(OrderProduct product)
        {
            var item = GetItem(product);
            if (item == null)
            {
                throw new DomainException($"No {product.Name} items in the basket to remove");
            }
            else
            {
                item.Remove();
            }
        }

        private BasketItem GetItem(OrderProduct product)
        {
            return _items.FirstOrDefault(e => e.Product == product);
        }

        public void Checkout()
        {
            if (Empty)
            {
                throw new DomainException("Cannot check-out as the basket is empty");
            }
            if (CheckedOut)
            {
                throw new DomainException("The basket is already checked-out");
            }
            CheckedOut = true;
            AddDomainEvent(new BasketCheckedOutDomainEvent(Id, CustomerId));
        }

        public void Clear()
        {
            _items.Clear();
        }
    }
}
