using DDDMart.Infrastructure;
using DDDMart.Payments.Core;
using DDDMart.Payments.Core.Invoices.Entities;
using DDDMart.Payments.Core.PaymentMethods.Entities;
using DDDMart.Payments.Infrastructure.Configurations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DDDMart.Payments.Infrastructure
{
    public class PaymentsContext : DbContextBase<PaymentsContext>
    {
        public PaymentsContext(DbContextOptions<PaymentsContext> options, IMediator mediator, IPaymentsIntegrationEventMapper eventMapper) : base(options, mediator, eventMapper)
        {
        }

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("payments");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(InvoiceConfiguration).Assembly);
        }
    }
}
