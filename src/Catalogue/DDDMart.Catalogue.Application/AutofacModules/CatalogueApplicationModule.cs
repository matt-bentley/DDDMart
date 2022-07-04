using Autofac;
using DDDMart.Catalogue.Application.Services;

namespace DDDMart.Catalogue.Application.AutofacModules
{
    public class CatalogueApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CatalogueIntegrationEventMapper>()
                   .AsImplementedInterfaces()
                   .SingleInstance();
        }
    }
}
