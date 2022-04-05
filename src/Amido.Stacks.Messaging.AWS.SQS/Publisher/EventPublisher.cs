using System.Text.Json;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Configuration;
using Amido.Stacks.Messaging.AWS.SQS.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Amido.Stacks.Messaging.AWS.SQS.Publisher
{
    /// <summary>
    /// Class implementing the ability to publish an event to AWS SQS
    /// </summary>
    public class EventPublisher : IApplicationEventPublisher
    {
        private readonly IOptions<AwsSqsConfiguration> configuration;
        private readonly ISecretResolver<string> secretResolver;
        private readonly IAmazonSQS queueClient;
        private readonly ILogger<EventPublisher> logger;

        public EventPublisher(
            IOptions<AwsSqsConfiguration> configuration,
            ISecretResolver<string> secretResolver,
            IAmazonSQS queueClient,
            ILogger<EventPublisher> logger)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.secretResolver = secretResolver ?? throw new ArgumentNullException(nameof(secretResolver));
            this.queueClient = queueClient ?? throw new ArgumentNullException(nameof(queueClient));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Publishes an event message to the configured SQS
        /// </summary>
        /// <param name="applicationEvent">The message object</param>
        /// <returns>Task</returns>
        public async Task PublishAsync(IApplicationEvent applicationEvent)
        {
            logger.PublishEventRequested(applicationEvent.CorrelationId.ToString());
            
            var queueUrl = await secretResolver.ResolveSecretAsync(configuration.Value.QueueUrl);
            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var eventReading = JsonSerializer.Serialize<object>(applicationEvent, jsonOptions);
            var messageRequest = new SendMessageRequest(queueUrl, eventReading);

            try
            {
                await queueClient.SendMessageAsync(messageRequest);
                
                logger.PublishEventCompleted(applicationEvent.CorrelationId.ToString());

            }
            catch (AmazonSQSException exception)
            {
                logger.PublishEventFailed(applicationEvent.CorrelationId.ToString(), exception.Message, exception);

            }
            catch (AmazonClientException exception)
            {
                logger.PublishEventFailed(applicationEvent.CorrelationId.ToString(), exception.Message, exception);
            }
        }
    }
}