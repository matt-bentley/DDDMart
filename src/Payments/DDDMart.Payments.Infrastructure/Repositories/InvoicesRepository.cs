using DDDMart.Infrastructure.Repositories;
using DDDMart.Payments.Core.Invoices.Entities;
using DDDMart.Payments.Core.Invoices.Repositories;

namespace DDDMart.Payments.Infrastructure.Repositories
{
    public class InvoicesRepository : Repository<Invoice, PaymentsContext>, IInvoicesRepository
    {
        public InvoicesRepository(PaymentsContext context) : base(context)
        {
        }
    }
}
