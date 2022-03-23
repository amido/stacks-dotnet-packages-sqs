# Amido Stacks Messaging AWS SQS

This library is wrapper around AWS SQS.
The main goal is:

    1.) to publish messages to a configured SQS queue
    2.) to receive messages from a configured SQS queue

## 1. Registration/Usage

### 1.1 Dependencies
- `Amido.Stacks.Configuration`
- `AWSSDK.SQS`

### 1.2 Currently Supported messages

The library currently supports:
- publishing and receiving events implementing `Amido.Stacks.Application.CQRS.ApplicationEvents.IApplicationEvent`

### 1.3 Usage in dotnet core application

#### 1.3.1 Event
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
#### 1.3.1.1 AWS Options Configuration

***appsettings.json***

```json
{
  "AwsSqsConfiguration": {
      "QueueUrl": "*SQS_URL*"
  }
}
```
***Usage***
```Startup.cs
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<AwsSqsConfiguration>(context.Configuration.GetSection("AwsSqsConfiguration"));
        services.AddAwsSqs();
    }
}
