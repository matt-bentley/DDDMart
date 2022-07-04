using DDDMart.Ordering.Core.Baskets.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDDMart.Ordering.Infrastructure.Configurations
{
    internal class BasketConfiguration : IEntityTypeConfiguration<Basket>
    {
        public void Configure(EntityTypeBuilder<Basket> builder)
        {
            builder.HasMany(e => e.Items)
                   .WithOne()
                   .HasForeignKey(e => e.BasketId);

            builder.HasIndex(e => e.CustomerId);
        }
    }

    internal class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
    {
        public void Configure(EntityTypeBuilder<BasketItem> builder)
        {
            builder.OwnsOne(e => e.Product);
        }
    }
}
