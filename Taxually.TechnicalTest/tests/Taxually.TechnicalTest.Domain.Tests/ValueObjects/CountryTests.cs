using Taxually.TechnicalTest.Domain.Exceptions;
using Taxually.TechnicalTest.Domain.ValueObjects;

namespace Taxually.TechnicalTest.Domain.Tests.ValueObjects
{
    [TestClass]
    public class CountryTests
    {
        [TestMethod]
        public void ShouldReturnCorrectCountryCode()
        {
            var code = "UK";

            var country = Country.From(code);

            country.Code.Should().Be(code);
        }

        [TestMethod]
        public void ToStringReturnsCode()
        {
            var country = Country.France;

            country.ToString().Should().Be(country.Code);
        }


        [TestMethod]
        public void ShouldThrowUnsupportedCountryExceptionGivenNotSupportedCountryCode()
        {
            FluentActions.Invoking(() => Country.From("ABCD"))
                .Should().Throw<UnsupportedCountryException>();
        }
    }

}
