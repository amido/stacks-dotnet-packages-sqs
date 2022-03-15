using Amazon.SQS;

namespace Amido.Stacks.SQS.Consumer
{
    public class EventConsumer : IEventConsumer
    {
        private readonly IAmazonSQS _awsClient;

        public EventConsumer(IAmazonSQS awsClient)
        {
            _awsClient = awsClient;
        }

        public Task ProcessAsync()
        {
            // consume from SQS
            throw new NotImplementedException();
        }
    }
}