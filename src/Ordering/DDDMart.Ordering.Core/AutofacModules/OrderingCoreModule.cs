using Autofac;
using MediatR;

namespace DDDMart.Ordering.Core.AutofacModules
{
    public class OrderingCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register the DomainEventHandler classes (they implement INotificationHandler<>) in assembly holding the Domain Events
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AsClosedTypesOf(typeof(INotificationHandler<>));
        }
    }
}
