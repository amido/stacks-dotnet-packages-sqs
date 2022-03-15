namespace Amido.Stacks.SQS.Consumer
{
    public interface IEventConsumer
    {
        Task ProcessAsync();
    }
}