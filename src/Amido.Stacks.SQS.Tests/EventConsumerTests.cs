using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Amido.Stacks.Configuration;
using Amido.Stacks.SQS.Consumer;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
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
            new EventConsumer(
                null, 
                A.Fake<ISecretResolver<string>>(),
                A.Fake<IAmazonSQS>(), 
                A.Fake<ILogger<EventConsumer>>());

        // assert
        constructor
            .Should()
            .Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'configuration')"); ;
    }

    [Fact]
    public void Given_ISecretResolverIsNull_Should_ThrowArgumentNullException()
    {
        // arrange
        // act
        Action constructor = () =>
            new EventConsumer(
                A.Fake<IOptions<AwsSqsConfiguration>>(), 
                null,
                A.Fake<IAmazonSQS>(), 
                A.Fake<ILogger<EventConsumer>>());

        // assert
        constructor
            .Should()
            .Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'secretResolver')"); ;
    }

    [Fact]
    public void Given_IAmazonSQSIsNull_Should_ThrowArgumentNullException()
    {
        // arrange
        // act
        Action constructor = () =>
            new EventConsumer(
                A.Fake<IOptions<AwsSqsConfiguration>>(), 
                A.Fake<ISecretResolver<string>>(),
                null, 
                A.Fake<ILogger<EventConsumer>>());

        // assert
        constructor
            .Should()
            .Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'queueClient')"); ;
    }
    
    [Fact]
    public void Given_ILoggerIsNull_Should_ThrowArgumentNullException()
    {
        // arrange
        // act
        Action constructor = () =>
            new EventConsumer(
                A.Fake<IOptions<AwsSqsConfiguration>>(), 
                A.Fake<ISecretResolver<string>>(),
                A.Fake<IAmazonSQS>(), 
                null);

        // assert
        constructor
            .Should()
            .Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'logger')"); ;
    }

    [Fact]
    public void Given_AllRequiredParameters_Should_NotThrow()
    {
        // arrange
        // act
        Action constructor = () =>
            new EventConsumer(
                A.Fake<IOptions<AwsSqsConfiguration>>(), 
                A.Fake<ISecretResolver<string>>(),
                A.Fake<IAmazonSQS>(), 
                A.Fake<ILogger<EventConsumer>>());

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
            QueueUrl = new Secret()
        };
        
        var fakeAmazonSqs = A.Fake<IAmazonSQS>();
        var fakeLogger = A.Fake<ILogger<EventConsumer>>();
        var fakeOptions = A.Fake<IOptions<AwsSqsConfiguration>>();
        A.CallTo(() => fakeOptions.Value).Returns(awsSqsConfiguration);
        
        var fakeSecretResolver = A.Fake<ISecretResolver<string>>();
        A.CallTo(() => fakeSecretResolver.ResolveSecretAsync(A<Secret>._)).Returns("QueueUrl");
        
        var sut = new EventConsumer(fakeOptions,fakeSecretResolver, fakeAmazonSqs, fakeLogger);
        
        // act
        await sut.ProcessAsync();

        // assert
        A.CallTo(() => fakeAmazonSqs.ReceiveMessageAsync(A<ReceiveMessageRequest>._, CancellationToken.None))
            .MustHaveHappenedOnceExactly();
    }
}