using DDDMart.Payments.Core.Tests.Builders;

namespace DDDMart.Payments.Core.Tests.PaymentMethods.Entities
{
    [TestClass]
    public class PaymentMethodTests
    {
        [TestMethod]
        public void GivenPaymentMethod_WhenCreate_ThenCreate()
        {
            var name = "Personal";
            var paymentMethod = new PaymentMethodBuilder().WithName(name).Build();
            paymentMethod.Name.Should().Be(name);
        }
    }
}
