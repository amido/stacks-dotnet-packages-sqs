using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Microsoft.Extensions.Options;

namespace Amido.Stacks.SQS.Publisher
{
    /// <summary>
    /// Class implementing the ability to publish an event to AWS SQS
    /// </summary>
    public class EventPublisher : IApplicationEventPublisher
    {
        private readonly IOptions<AwsSqsConfiguration> _configuration;
        private readonly IAmazonSQS _queueClient;

        public EventPublisher(
            IOptions<AwsSqsConfiguration> configuration,
            IAmazonSQS queueClient)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _queueClient = queueClient ?? throw new ArgumentNullException(nameof(queueClient));
        }

        /// <summary>
        /// Publishes an event message to the configured SQS
        /// </summary>
        /// <param name="applicationEvent">The message object</param>
        /// <returns>Task</returns>
        public async Task PublishAsync(IApplicationEvent applicationEvent)
        {
            var eventReading = JsonSerializer.Serialize(applicationEvent);
            var messageRequest = new SendMessageRequest(_configuration.Value.QueueUrl, 
                eventReading);
            await _queueClient.SendMessageAsync(messageRequest);
        }
    }
}