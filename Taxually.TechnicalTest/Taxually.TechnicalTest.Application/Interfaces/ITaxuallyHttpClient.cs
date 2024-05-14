namespace Taxually.TechnicalTest.Application.Interfaces
{
    public interface ITaxuallyHttpClient
    {
        Task PostAsync<TRequest>(string url, TRequest request);
    }
}
