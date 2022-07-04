using MediatR;

namespace DDDMart.SharedKernel
{
    public abstract record DomainEvent : INotification;
}
