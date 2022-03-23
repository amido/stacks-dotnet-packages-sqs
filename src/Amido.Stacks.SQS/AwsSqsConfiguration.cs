using Amido.Stacks.Configuration;

namespace Amido.Stacks.SQS;

public class AwsSqsConfiguration
{
    public Secret QueueUrl { get; init; }
}