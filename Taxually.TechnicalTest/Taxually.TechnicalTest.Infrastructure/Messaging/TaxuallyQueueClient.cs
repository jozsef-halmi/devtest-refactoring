using Taxually.TechnicalTest.Application.Interfaces;

namespace Taxually.TechnicalTest.Infrastructure.Messaging
{
    public class TaxuallyQueueClient : ITaxuallyQueueClient
    {
        public Task EnqueueAsync<TPayload>(string queueName, TPayload payload)
        {
            // Code to send to message queue removed for brevity
            return Task.CompletedTask;
        }
    }
}
