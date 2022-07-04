using DDDMart.Application;
using DDDMart.Catalogue.Core;
using DDDMart.SharedKernel;

namespace DDDMart.Catalogue.Application.Services
{
    public class CatalogueIntegrationEventMapper : IntegrationEventMapper, ICatalogueIntegrationEventMapper
    {
        protected override IntegrationEvent MapDomainEvent<T>(T domainEvent)
        {
            return null;
        }
    }
}
