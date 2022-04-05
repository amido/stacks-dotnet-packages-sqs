namespace Amido.Stacks.Messaging.AWS.SQS.Consumer
{
    public interface IEventConsumer
    {
        Task ProcessAsync();
    }
}