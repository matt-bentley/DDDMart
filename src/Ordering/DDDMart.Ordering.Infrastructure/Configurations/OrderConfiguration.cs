using DDDMart.Ordering.Core.Orders.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDDMart.Ordering.Infrastructure.Configurations
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(e => e.ShippingAddress);
            builder.OwnsOne(e => e.PaymentAddress);

            builder.HasIndex(e => e.CustomerId);
            builder.HasIndex(e => e.BasketId).IsUnique();

            builder.HasMany(e => e.Items)
                   .WithOne()
                   .HasForeignKey(e => e.OrderId);
        }
    }

    internal class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.OwnsOne(e => e.Product);
        }
    }
}
