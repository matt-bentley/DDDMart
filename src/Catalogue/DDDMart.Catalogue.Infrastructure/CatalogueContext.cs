using DDDMart.Catalogue.Core;
using DDDMart.Catalogue.Core.Products.Entities;
using DDDMart.Catalogue.Infrastructure.Configurations;
using DDDMart.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DDDMart.Catalogue.Infrastructure
{
    public class CatalogueContext : DbContextBase<CatalogueContext>
    {
        public CatalogueContext(DbContextOptions<CatalogueContext> options, IMediator mediator, ICatalogueIntegrationEventMapper eventMapper) : base(options, mediator, eventMapper)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("catalogue");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);
        }
    }
}
