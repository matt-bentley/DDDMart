using DDDMart.Infrastructure.Repositories;
using DDDMart.Payments.Core.PaymentMethods.Entities;
using DDDMart.Payments.Core.PaymentMethods.Repositories;

namespace DDDMart.Payments.Infrastructure.Repositories
{
    public class PaymentMethodsRepository : Repository<PaymentMethod, PaymentsContext>, IPaymentMethodsRepository
    {
        public PaymentMethodsRepository(PaymentsContext context) : base(context)
        {
        }
    }
}
