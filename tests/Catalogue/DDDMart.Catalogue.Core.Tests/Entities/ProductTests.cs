using DDDMart.Catalogue.Core.Tests.Builders;
using DDDMart.SharedKernel.Exceptions;

namespace DDDMart.Catalogue.Core.Tests.Entities
{
    [TestClass]
    public class ProductTests
    {
        [TestMethod]
        public void GivenProduct_WhenSameId_ThenEquals()
        {
            var id = Guid.NewGuid();
            var product1 = new ProductBuilder().WithId(id).Build();
            var product2 = new ProductBuilder().WithId(id).Build();
            product1.Should().Be(product2);
            product1.GetHashCode().Should().Be(product2.GetHashCode());
        }

        [TestMethod]
        public void GivenProduct_WhenCreate_ThenCreate()
        {
            var name = "Test";
            var product = new ProductBuilder().WithName(name).Build();
            product.Info.Name.Should().Be(name);
        }

        [TestMethod]
        public void GivenProduct_WhenPriceLessThanZero_ThenThrow()
        {
            Action action = () => _ = new ProductBuilder().WithPrice(-10).Build();
            action.Should().Throw<DomainException>().WithMessage("'Price' must be greater than or equal to 0.");
        }
    }
}
