using DDDMart.Infrastructure;
using DDDMart.Ordering.Core;
using DDDMart.Ordering.Core.Baskets.Entities;
using DDDMart.Ordering.Core.Orders.Entities;
using DDDMart.Ordering.Infrastructure.Configurations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DDDMart.Ordering.Infrastructure
{
    public class OrderingContext : DbContextBase<OrderingContext>
    {
        public OrderingContext(DbContextOptions<OrderingContext> options, IMediator mediator, IOrderingIntegrationEventMapper eventMapper) : base(options, mediator, eventMapper)
        {
        }

        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("baskets");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BasketConfiguration).Assembly);
        }
    }
}