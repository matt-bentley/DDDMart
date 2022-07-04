using DDDMart.Payments.Core.Invoices.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDDMart.Payments.Infrastructure.Configurations
{
    internal class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.OwnsOne(e => e.Address);

            builder.HasIndex(e => e.CustomerId);
            builder.HasIndex(e => e.OrderId).IsUnique();
        }
    }
}
