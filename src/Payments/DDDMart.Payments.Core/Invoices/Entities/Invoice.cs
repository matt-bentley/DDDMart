using DDDMart.Payments.Core.Invoices.DomainEvents;
using DDDMart.Payments.Core.Invoices.ValueObjects;
using DDDMart.SharedKernel;
using DDDMart.SharedKernel.Exceptions;

namespace DDDMart.Payments.Core.Invoices.Entities
{
    public class Invoice : AggregateRoot
    {
        private Invoice(Guid customerId, Guid orderId, Guid paymentMethodId, InvoiceAddress invoiceAddress, decimal amount, InvoiceStatus status, DateTime sentDate, DateTime? paidDate)
        {
            CustomerId = customerId;
            OrderId = orderId;
            PaymentMethodId = paymentMethodId;
            Amount = amount;
            Status = status;
            SentDate = sentDate;
            PaidDate = paidDate;
            Address = invoiceAddress;
        }

        private Invoice()
        {

        }

        public static Invoice Generate(Guid customerId, Guid orderId, Guid paymentMethodId, InvoiceAddress invoiceAddress, decimal amount)
        {
            return new Invoice(customerId, orderId, paymentMethodId, invoiceAddress, amount, InvoiceStatus.NotPaid, DateTime.UtcNow, null);
        }

        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }
        public InvoiceAddress Address { get; private set; }
        public Guid PaymentMethodId { get; private set; }
        public decimal Amount { get; private set; }
        public InvoiceStatus Status { get; private set; }
        public DateTime SentDate { get; private set; }
        public DateTime? PaidDate { get; private set; }
        public bool Paid => PaidDate.HasValue;

        public void Pay()
        {
            if (Paid)
            {
                throw new DomainException($"Invoice has already been paid on {PaidDate.Value}");
            }
            Status = InvoiceStatus.Paid;
            PaidDate = DateTime.UtcNow;
            AddDomainEvent(new InvoicePaidDomainEvent(Id, PaidDate.Value, OrderId, CustomerId));
        }
    }
}
