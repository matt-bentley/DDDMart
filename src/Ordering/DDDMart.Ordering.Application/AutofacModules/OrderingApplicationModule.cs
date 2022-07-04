using Autofac;
using DDDMart.Ordering.Application.Services;

namespace DDDMart.Ordering.Application.AutofacModules
{
    public class OrderingApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<OrderingIntegrationEventMapper>()
                   .AsImplementedInterfaces()
                   .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(e => e.Name.EndsWith("IntegrationEventHandler"));
        }
    }
}
