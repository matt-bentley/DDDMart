using DDDMart.Payments.Core.Invoices.DomainEvents;
using DDDMart.Payments.Core.Invoices.ValueObjects;
using DDDMart.Payments.Core.Tests.Builders;

namespace DDDMart.Payments.Core.Tests.Invoices.Entities
{
    [TestClass]
    public class InvoiceTests
    {
        [TestMethod]
        public void GivenInvoice_WhenCreate_ThenCreateNotPaid()
        {
            var invoice = new InvoiceBuilder().Build();
            invoice.Should().NotBeNull();
            invoice.Status.Should().Be(InvoiceStatus.NotPaid);
            invoice.Paid.Should().BeFalse();
            invoice.PaidDate.Should().BeNull();
        }

        [TestMethod]
        public void GivenInvoice_WhenPay_ThenPay()
        {
            var invoice = new InvoiceBuilder().Build();
            invoice.Pay();
            invoice.Status.Should().Be(InvoiceStatus.Paid);
            invoice.Paid.Should().BeTrue();
            invoice.PaidDate.Should().NotBeNull();
            invoice.DomainEvents.Where(e => e is InvoicePaidDomainEvent).Should().HaveCount(1);
        }
    }
}
