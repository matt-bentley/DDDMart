
namespace DDDMart.SharedKernel
{
    public abstract class IntegrationEvent
    {
        protected IntegrationEvent() : this(Guid.NewGuid())
        {
            CreatedDate = DateTime.Now;
        }

        protected IntegrationEvent(Guid id)
        {
            Id = id;
            CreatedDate = DateTime.Now;
        }

        public readonly Guid Id;
        public readonly DateTime CreatedDate;
    }
}
