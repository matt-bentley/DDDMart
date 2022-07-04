using DDDMart.SharedKernel;

namespace DDDMart.Payments.Core.Invoices.DomainEvents
{
    public record InvoicePaidDomainEvent(Guid Id, DateTime PaidDate, Guid OrderId, Guid CustomerId) : DomainEvent;
}
