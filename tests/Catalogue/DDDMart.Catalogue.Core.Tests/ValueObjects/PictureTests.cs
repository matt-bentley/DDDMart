using DDDMart.Catalogue.Core.ValueObjects;
using DDDMart.SharedKernel.Exceptions;

namespace DDDMart.Catalogue.Core.Tests.ValueObjects
{
    [TestClass]
    public class PictureTests
    {
        [TestMethod]
        public void GivenPicture_WhenCreate_ThenCreate()
        {
            var info = Picture.Create("name", "https://test.com");
            info.Name.Should().Be("name");
        }

        [TestMethod]
        public void GivenPicture_WhenInvalidUrl_ThenThrow()
        {
            Action action = () => Picture.Create("name", "https:test.com");
            action.Should().Throw<DomainException>().WithMessage("Must have a valid Uri.");
        }

        [TestMethod]
        public void GivenPicture_WhenCreateSameValue_ThenEquals()
        {
            var info1 = Picture.Create("name", "https://test.com");
            var info2 = Picture.Create("name", "https://test.com");
            info1.Should().Be(info2);
            info1.GetHashCode().Should().Be(info2.GetHashCode());
        }
    }
}
