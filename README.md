# Amido Stacks Messaging AWS SQS

This library is wrapper around AWS SQS.
The main goal is:

    1.) to publish messages to a configured SQS queue
    2.) to receive messages from a configured SQS queue

## 1. Requirements

You need a SQS instance in order to use this library. You can follow the official instructions provided by AWS [here](https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/sqs-apis-intro.html).

## 2. Registration/Usage

### 2.1 Dependencies
- `Amido.Stacks.Configuration`
- `AWSSDK.SQS`

### 2.2 Currently Supported messages

The library currently supports:
- publishing and receiving events implementing `Amido.Stacks.Application.CQRS.ApplicationEvents.IApplicationEvent`

### 2.3 Usage in dotnet core application

#### 2.3.1 Event
In this case the `NotifyEvent` has a `NotifyEventHandler`. The handler implements
`Amido.Stacks.Application.CQRS.ApplicationEvents.IApplicationEventHandler<NotifyCommand, bool>` and the command implements
`Amido.Stacks.Application.CQRS.ApplicationEvents.IApplicationEvent` interfaces.

***NotifyEvent.cs***

```cs
   public class NotifyEvent : IApplicationEvent
    {
        public int OperationCode { get; }
        public Guid CorrelationId { get; }
        public int EventCode { get; }

        public NotifyEvent(int operationCode, Guid correlationId, int eventCode)
        {
            OperationCode = operationCode;
            CorrelationId = correlationId;
            EventCode = eventCode;
        }
    }
```

***NotifyEventHandler.cs***

```cs
     public class NotifyEventHandler : IApplicationEventHandler<NotifyEvent>
     {
         private readonly ITestable<NotifyEvent> _testable;

         public NotifyEventHandler(ITestable<NotifyEvent> testable)
         {
             _testable = testable;
         }

         public Task HandleAsync(NotifyEvent applicationEvent)
         {
            _testable.Complete(applicationEvent);
            return Task.CompletedTask;
         }
     }
```
#### 2.3.1.1 AWS Options Configuration

***appsettings.json***

```json
{
  "AwsSqsConfiguration": {
      "QueueUrl": "SQS_QUEUE_URL"
  }
}
```
***Usage***
```cs
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<AwsSqsConfiguration>(context.Configuration.GetSection("AwsSqsConfiguration"));
        services.AddAwsSqs();
    }
}
```