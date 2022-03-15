using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amazon.SQS;

namespace Amido.Stacks.Messaging.Azure.EventHub.Publisher
{
    public class EventPublisher : IApplicationEventPublisher
    {
        private readonly IAmazonSQS _awsClient;

        public EventPublisher(IAmazonSQS awsClient)
        {
            _awsClient = awsClient;
        }

        public async Task PublishAsync(IApplicationEvent applicationEvent)
        {
           // publish event to SQS
        }
    }
}