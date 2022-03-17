using System.Text.Json;
using Amazon.Extensions.NETCore.Setup;
using Amazon.SQS;
using Amazon.SQS.Model;
using Amido.Stacks.Application.CQRS.ApplicationEvents;

namespace Amido.Stacks.SQS.Publisher
{
    /// <summary>
    /// Class implementing the ability to publish an event to AWS SQS
    /// </summary>
    public class EventPublisher : IApplicationEventPublisher
    {
        private readonly IAmazonSQS _queueClient;
        private readonly AWSOptions _awsOptions;

        public EventPublisher(
            IAmazonSQS queueClient,
            AWSOptions awsOptions)
        {
            _queueClient = queueClient;
            _awsOptions = awsOptions;
        }

        public async Task PublishAsync(IApplicationEvent applicationEvent)
        {
            /*
             * serialise message
             * create message request
             * publish message to queue
             */
            var eventReading = JsonSerializer.Serialize(applicationEvent);
            var messageRequest = new SendMessageRequest(_awsOptions.DefaultClientConfig.ServiceURL, 
                eventReading);
            await _queueClient.SendMessageAsync(messageRequest);
            
            throw new NotImplementedException();
        }
    }
}