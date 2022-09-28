using DDDMart.Catalogue.Core.Products.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDDMart.Catalogue.Infrastructure.Configurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.OwnsOne(e => e.Info, infoBuilder =>
            {
                infoBuilder.HasIndex(e => e.Name).IsUnique();
            });

            builder.HasIndex(e => e.SicCode).IsUnique();
            builder.OwnsOne(e => e.Picture);
        }
    }
}
