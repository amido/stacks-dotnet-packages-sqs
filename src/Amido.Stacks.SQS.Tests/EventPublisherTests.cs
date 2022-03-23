using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.SQS.Publisher;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;

namespace Amido.Stacks.SQS.Tests;

public class EventPublisherTests
{
    [Fact]
    public void Should_BeDerivedFromIApplicationEventPublisher()
    {
        // arrange
        // act
        // assert
        typeof(EventPublisher)
            .Should()
            .Implement<IApplicationEventPublisher>();
    }

    [Fact]
    public void Given_IOptionsIsNull_Should_ThrowArgumentNullException()
    {
        // arrange
        // act
        Action constructor = () =>
            new EventPublisher(null, A.Fake<IAmazonSQS>());

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
            new EventPublisher(A.Fake<IOptions<AwsSqsConfiguration>>(), null);

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
            new EventPublisher(A.Fake<IOptions<AwsSqsConfiguration>>(), A.Fake<IAmazonSQS>());

        // assert
        constructor
            .Should()
            .NotThrow();
    }

    [Fact]
    public async Task PublishAsync_Should_MakeCallToSendMessageAsync()
    {
        // arrange
        var awsSqsConfiguration = new AwsSqsConfiguration
        {
            QueueUrl = "QueueUrl"
        };
        var fakeApplicationEvent = A.Fake<IApplicationEvent>();
        
        var fakeAmazonSqs = A.Fake<IAmazonSQS>();
        var fakeOptions = A.Fake<IOptions<AwsSqsConfiguration>>();
        A.CallTo(() => fakeOptions.Value).Returns(awsSqsConfiguration);
        
        var sut = new EventPublisher(fakeOptions, fakeAmazonSqs);
        
        // act
        await sut.PublishAsync(fakeApplicationEvent);

        // assert
        A.CallTo(() => fakeAmazonSqs.SendMessageAsync(A<SendMessageRequest>._, CancellationToken.None))
            .MustHaveHappenedOnceExactly();
    }
}