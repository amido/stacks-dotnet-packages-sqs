using Amazon.SQS;
using Amazon.SQS.Model;
using Amido.Stacks.Configuration;
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
        private readonly ISecretResolver<string> _secretResolver;

        public EventConsumer(
            IOptions<AwsSqsConfiguration> configuration,
            ISecretResolver<string> secretResolver,
            IAmazonSQS queueClient)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _secretResolver = secretResolver ?? throw new ArgumentNullException(nameof(secretResolver));
            _queueClient = queueClient ?? throw new ArgumentNullException(nameof(queueClient));
        }

        /// <summary>
        /// Retrieves events from the configured SQS
        /// </summary>
        /// <returns>Task</returns>
        public async Task ProcessAsync()
        {
            var queueUrl = await _secretResolver.ResolveSecretAsync(_configuration.Value.QueueUrl);
            var messageRequest = new ReceiveMessageRequest(queueUrl);
            await _queueClient.ReceiveMessageAsync(messageRequest);
        }
    }
}