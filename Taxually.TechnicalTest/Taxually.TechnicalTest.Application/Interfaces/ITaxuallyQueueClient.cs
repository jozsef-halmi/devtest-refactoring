namespace Taxually.TechnicalTest.Application.Interfaces
{
    public interface ITaxuallyQueueClient
    {
        Task EnqueueAsync<TPayload>(string queueName, TPayload payload);
    }
}
