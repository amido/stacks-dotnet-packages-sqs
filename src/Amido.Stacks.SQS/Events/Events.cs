namespace Amido.Stacks.SQS.Events;

public enum EventCode
{
    Initializing = 123456800,
    GeneralException = 123456899,
    
    PublishEventRequested = 123456801,
    PublishEventCompleted = 123456802,
    PublishEventFailed = 123456803,

    ProcessEventsRequested = 123456811,
    ProcessEventsCompleted = 123456812,
    ProcessEventsFailed = 123456813,
}