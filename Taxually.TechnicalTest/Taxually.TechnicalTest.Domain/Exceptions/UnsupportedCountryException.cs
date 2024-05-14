namespace Taxually.TechnicalTest.Domain.Exceptions
{
    public class UnsupportedCountryException : Exception
    {
        public UnsupportedCountryException(string code)
            : base($"Country \"{code}\" is unsupported.")
        {
        }
    }

}
