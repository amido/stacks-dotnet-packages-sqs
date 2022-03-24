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
        private readonly IOptions<AwsSqsConfiguration> configuration;
        private readonly ISecretResolver<string> secretResolver;
        private readonly IAmazonSQS queueClient;

        public EventPublisher(
            IOptions<AwsSqsConfiguration> configuration,
            ISecretResolver<string> secretResolver,
            IAmazonSQS queueClient)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.secretResolver = secretResolver ?? throw new ArgumentNullException(nameof(secretResolver));
            this.queueClient = queueClient ?? throw new ArgumentNullException(nameof(queueClient));
        }

        /// <summary>
        /// Publishes an event message to the configured SQS
        /// </summary>
        /// <param name="applicationEvent">The message object</param>
        /// <returns>Task</returns>
        public async Task PublishAsync(IApplicationEvent applicationEvent)
        {
            var queueUrl = await secretResolver.ResolveSecretAsync(configuration.Value.QueueUrl);
            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var eventReading = JsonSerializer.Serialize<object>(applicationEvent, jsonOptions);
            var messageRequest = new SendMessageRequest(queueUrl, eventReading);
            await queueClient.SendMessageAsync(messageRequest);
        }
    }
}