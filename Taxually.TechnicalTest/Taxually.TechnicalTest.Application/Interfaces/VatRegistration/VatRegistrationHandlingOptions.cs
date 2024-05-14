namespace Taxually.TechnicalTest.Application.Interfaces.VatRegistration
{
    public sealed class VatRegistrationHandlingOptions
    {
        public string GermanyQueueName { get; set; }
        public string FranceQueueName { get; set; }
        public string UkApiBaseUrl { get; set; }
    }
}
