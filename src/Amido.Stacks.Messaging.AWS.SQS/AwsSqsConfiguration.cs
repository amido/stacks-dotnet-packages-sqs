using Amido.Stacks.Configuration;

namespace Amido.Stacks.Messaging.AWS.SQS;

public class AwsSqsConfiguration
{
    public Secret QueueUrl { get; init; }
}