using DDDMart.Infrastructure;
using DDDMart.SharedKernel;

namespace MediatR
{
    internal static class MediatorExtensions
    {
        public static async Task DispatchEventsAsync<T>(this IMediator mediator, DbContextBase<T> context) where T : DbContextBase<T>
        {
            var aggregateRoots = context.ChangeTracker
                .Entries<AggregateRoot>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
                .Select(e => e.Entity)
                .ToList();

            var domainEvents = aggregateRoots
                .SelectMany(x => x.DomainEvents)
                .ToList();

            await mediator.DispatchDomainEventsAsync(domainEvents);
            await DispatchIntegrationEventsAsync(domainEvents, context);

            ClearDomainEvents(aggregateRoots);
        }

        private static async Task DispatchDomainEventsAsync(this IMediator mediator, List<DomainEvent> domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent);
            }
        }

        private static async Task DispatchIntegrationEventsAsync<T>(IEnumerable<DomainEvent> domainEvents, DbContextBase<T> context) where T : DbContextBase<T>
        {
            var integrationEvents = context.EventMapper.Map(domainEvents);
            if (integrationEvents != null)
            {
                await context.OutboxIntegrationEvents.AddRangeAsync(integrationEvents);
            }
        }

        private static void ClearDomainEvents(List<AggregateRoot> aggregateRoots)
        {
            aggregateRoots.ForEach(aggregate => aggregate.ClearDomainEvents());
        }
    }
}
