using System.Diagnostics.Metrics;
using Taxually.TechnicalTest.Domain.Exceptions;

namespace Taxually.TechnicalTest.Domain.ValueObjects
{
    public class Country : ValueObject
    {
        public string Code { get; private set; }

        private Country(string code)
        {
            Code = code;
        }

        public static Country From(string code)
        {
            var country = new Country(code);

            if (!SupportedCountrys.Contains(country))
            {
                throw new UnsupportedCountryException(code);
            }

            return country;
        }

        public static Country France => new("FR");
        public static Country Germany => new("DE");
        public static Country UnitedKingdom => new("UK");

        public override string ToString()
        {
            return Code;
        }

        protected static IEnumerable<Country> SupportedCountrys
        {
            get
            {
                yield return France;
                yield return Germany;
                yield return UnitedKingdom;
            }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
        }
    }
}


