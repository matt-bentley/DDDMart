using Autofac;
using DDDMart.Application.EventBus;

namespace DDDMart.Application.AutofacModules
{
    public class EventBusModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<InMemoryEventBus>()
                   .AsImplementedInterfaces()
                   .SingleInstance();

            builder.RegisterType<SubscriptionContainer>()
                   .AsImplementedInterfaces()
                   .SingleInstance();
        }
    }
}
