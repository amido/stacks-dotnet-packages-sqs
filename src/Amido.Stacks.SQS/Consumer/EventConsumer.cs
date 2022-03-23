using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;

namespace Amido.Stacks.SQS.Consumer
{
    /// <summary>
    /// Class implementing the ability to consume to an event on AWS SQS
    /// </summary>
    public class EventConsumer : IEventConsumer
    {
        private readonly IAmazonSQS _queueClient;
        private readonly IOptions<AwsSqsConfiguration> _configuration;

        public EventConsumer(
            IOptions<AwsSqsConfiguration> configuration,
            IAmazonSQS queueClient)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _queueClient = queueClient ?? throw new ArgumentNullException(nameof(queueClient));
        }

        /// <summary>
        /// Retrieves events from the configured SQS
        /// </summary>
        /// <returns>Task</returns>
        public async Task ProcessAsync()
        {
            var messageRequest = new ReceiveMessageRequest(_configuration.Value.QueueUrl);
            await _queueClient.ReceiveMessageAsync(messageRequest);
        }
    }
}