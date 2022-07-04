using DDDMart.SharedKernel;
using DDDMart.SharedKernel.Outbox.Entities;
using DDDMart.SharedKernel.Outbox.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DDDMart.Infrastructure
{
    public abstract class DbContextBase<T> : DbContext, IUnitOfWork where T : DbContextBase<T>
    {
        private readonly IMediator _mediator;
        public readonly IIntegrationEventMapper EventMapper;

        protected DbContextBase(DbContextOptions<T> options,
            IMediator mediator,
            IIntegrationEventMapper eventMapper) : base(options)
        {
            _mediator = mediator;
            EventMapper = eventMapper;
        }

        public DbSet<OutboxIntegrationEvent> OutboxIntegrationEvents { get; set; }

        public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
        {
            // Dispatch Domain Events collection. 
            // Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            // Integration Events will be stored in the IntegrationEventOutbox ready to be published later
            await _mediator.DispatchEventsAsync(this);

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed
            var result = await base.SaveChangesAsync(cancellationToken);

            return true;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            // used to print SQL when debugging
            optionsBuilder.UseLoggerFactory(new LoggerFactory(new[] { new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider() }));
#endif
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore<AggregateRoot>();
            modelBuilder.Entity<AggregateRoot>().Ignore(e => e.DomainEvents);
            modelBuilder.Entity<AggregateRoot>().Property(q => q.Id).ValueGeneratedNever();
        }
    }
}