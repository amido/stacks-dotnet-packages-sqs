using Amazon.Extensions.NETCore.Setup;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Amido.Stacks.SQS.Consumer
{
    /// <summary>
    /// Class implementing the ability to consume to an event on AWS SQS
    /// </summary>
    public class EventConsumer : IEventConsumer
    {
        private readonly IAmazonSQS _queueClient;
        private readonly AWSOptions _awsOptions;

        public EventConsumer(IAmazonSQS queueClient, AWSOptions awsOptions)
        {
            _queueClient = queueClient;
            _awsOptions = awsOptions;
        }

        public async Task ProcessAsync()
        {
            /*
            * create message request
            * retrieve messages from queue
            */
            var messageRequest = new ReceiveMessageRequest(_awsOptions.DefaultClientConfig.ServiceURL);
            await _queueClient.ReceiveMessageAsync(messageRequest);
            
            throw new NotImplementedException();
        }
    }
}