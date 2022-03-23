using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Amido.Stacks.SQS.Consumer;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;

namespace Amido.Stacks.SQS.Tests;

public class EventConsumerTests
{
    [Fact]
    public void Should_BeDerivedFromIApplicationEventPublisher()
    {
        // arrange
        // act
        // assert
        typeof(EventConsumer)
            .Should()
            .Implement<IEventConsumer>();
    }

    [Fact]
    public void Given_IOptionsIsNull_Should_ThrowArgumentNullException()
    {
        // arrange
        // act
        Action constructor = () =>
            new EventConsumer(null, A.Fake<IAmazonSQS>());

        // assert
        constructor
            .Should()
            .Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'configuration')"); ;
    }

    [Fact]
    public void Given_IAmazonSQSIsNull_Should_ThrowArgumentNullException()
    {
        // arrange
        // act
        Action constructor = () =>
            new EventConsumer(A.Fake<IOptions<AwsSqsConfiguration>>(), null);

        // assert
        constructor
            .Should()
            .Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'queueClient')"); ;
    }

    [Fact]
    public void Given_AllRequiredParameters_Should_NotThrow()
    {
        // arrange
        // act
        Action constructor = () =>
            new EventConsumer(A.Fake<IOptions<AwsSqsConfiguration>>(), A.Fake<IAmazonSQS>());

        // assert
        constructor
            .Should()
            .NotThrow();
    }
    
    [Fact]
    public async Task ProcessAsync_Should_MakeCallToReceiveMessageAsync()
    {
        // arrange
        var awsSqsConfiguration = new AwsSqsConfiguration
        {
            QueueUrl = "QueueUrl"
        };
        
        var fakeAmazonSqs = A.Fake<IAmazonSQS>();
        var fakeOptions = A.Fake<IOptions<AwsSqsConfiguration>>();
        A.CallTo(() => fakeOptions.Value).Returns(awsSqsConfiguration);
        
        var sut = new EventConsumer(fakeOptions, fakeAmazonSqs);
        
        // act
        await sut.ProcessAsync();

        // assert
        A.CallTo(() => fakeAmazonSqs.ReceiveMessageAsync(A<ReceiveMessageRequest>._, CancellationToken.None))
            .MustHaveHappenedOnceExactly();
    }
}