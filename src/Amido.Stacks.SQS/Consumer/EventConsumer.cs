using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Amido.Stacks.Configuration;
using Amido.Stacks.SQS.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Amido.Stacks.SQS.Consumer
{
    /// <summary>
    /// Class implementing the ability to consume to an event on AWS SQS
    /// </summary>
    public class EventConsumer : IEventConsumer
    {
        private readonly IAmazonSQS queueClient;
        private readonly IOptions<AwsSqsConfiguration> configuration;
        private readonly ISecretResolver<string> secretResolver;
        private readonly ILogger<EventConsumer> logger;

        public EventConsumer(
            IOptions<AwsSqsConfiguration> configuration,
            ISecretResolver<string> secretResolver,
            IAmazonSQS queueClient,
            ILogger<EventConsumer> logger)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.secretResolver = secretResolver ?? throw new ArgumentNullException(nameof(secretResolver));
            this.queueClient = queueClient ?? throw new ArgumentNullException(nameof(queueClient));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retrieves events from the configured SQS
        /// </summary>
        /// <returns>Task</returns>
        public async Task ProcessAsync()
        {
            logger.ProcessEventsRequested();

            var queueUrl = await secretResolver.ResolveSecretAsync(configuration.Value.QueueUrl);
            var messageRequest = new ReceiveMessageRequest(queueUrl);
            try
            {
                var response = await queueClient.ReceiveMessageAsync(messageRequest);
                
                foreach (var message in response.Messages)
                {
                    logger.ProcessEventsCompleted(message.Body);
                }
            }
            catch (AmazonSQSException exception)
            {
                logger.ProcessEventsFailed(exception.Message, exception);
            }
            catch (AmazonClientException exception)
            {
                logger.ProcessEventsFailed(exception.Message, exception);
            }
        }
    }
}