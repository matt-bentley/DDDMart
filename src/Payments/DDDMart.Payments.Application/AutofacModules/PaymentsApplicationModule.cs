using Autofac;
using DDDMart.Payments.Application.Services;

namespace DDDMart.Payments.Application.AutofacModules
{
    public class PaymentsApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PaymentsIntegrationEventMapper>()
                   .AsImplementedInterfaces()
                   .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                   .Where(e => e.Name.EndsWith("IntegrationEventHandler"));
        }
    }
}
