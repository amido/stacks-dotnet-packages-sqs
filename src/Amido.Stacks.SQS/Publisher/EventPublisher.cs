using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Configuration;
using Microsoft.Extensions.Options;

namespace Amido.Stacks.SQS.Publisher
{
    /// <summary>
    /// Class implementing the ability to publish an event to AWS SQS
    /// </summary>
    public class EventPublisher : IApplicationEventPublisher
    {
        private readonly IOptions<AwsSqsConfiguration> _configuration;
        private readonly ISecretResolver<string> _secretResolver;
        private readonly IAmazonSQS _queueClient;

        public EventPublisher(
            IOptions<AwsSqsConfiguration> configuration,
            ISecretResolver<string> secretResolver,
            IAmazonSQS queueClient)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _secretResolver = secretResolver ?? throw new ArgumentNullException(nameof(secretResolver));
            _queueClient = queueClient ?? throw new ArgumentNullException(nameof(queueClient));
        }

        /// <summary>
        /// Publishes an event message to the configured SQS
        /// </summary>
        /// <param name="applicationEvent">The message object</param>
        /// <returns>Task</returns>
        public async Task PublishAsync(IApplicationEvent applicationEvent)
        {
            var queueUrl = await _secretResolver.ResolveSecretAsync(_configuration.Value.QueueUrl);
            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var eventReading = JsonSerializer.Serialize<object>(applicationEvent, jsonOptions);
            var messageRequest = new SendMessageRequest(queueUrl, eventReading);
            await _queueClient.SendMessageAsync(messageRequest);
        }
    }
}