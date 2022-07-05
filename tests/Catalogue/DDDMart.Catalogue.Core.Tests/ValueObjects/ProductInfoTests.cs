using DDDMart.Catalogue.Core.ValueObjects;
using DDDMart.SharedKernel.Exceptions;

namespace DDDMart.Catalogue.Core.Tests.ValueObjects
{
    [TestClass]
    public class ProductInfoTests
    {
        [TestMethod]
        public void GivenProductInfo_WhenCreate_ThenCreate()
        {
            var info = ProductInfo.Create("name", "description");
            info.Name.Should().Be("name");
        }

        [TestMethod]
        public void GivenProductInfo_WhenMissingName_ThenThrow()
        {
            Action action = () => ProductInfo.Create("", "description");
            action.Should().Throw<DomainException>().WithMessage("Required input 'Name' is missing.");
        }

        [TestMethod]
        public void GivenProductInfo_WhenCreateSameValue_ThenEquals()
        {
            var info1 = ProductInfo.Create("name", "description");
            var info2 = ProductInfo.Create("name", "description");
            info1.Should().Be(info2);
            info1.GetHashCode().Should().Be(info2.GetHashCode());
        }

        [TestMethod]
        public void GivenProductInfo_WhenCreateDifferentValue_ThenNotEquals()
        {
            var info1 = ProductInfo.Create("name", "description");
            var info2 = ProductInfo.Create("name2", "description2");
            info1.Should().NotBe(info2);
            info1.GetHashCode().Should().NotBe(info2.GetHashCode());
        }
    }
}
