using Amazon.SQS;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.Messaging.AWS.SQS.Consumer;
using Amido.Stacks.Messaging.AWS.SQS.Publisher;
using Microsoft.Extensions.DependencyInjection;

namespace Amido.Stacks.Messaging.AWS.SQS.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the AWS SQS client for IEventConsumer and IApplicationEventPublisher
        /// </summary>
        public static IServiceCollection AddAwsSqs(this IServiceCollection services)
        {
            services.AddAWSService<IAmazonSQS>();
            services.AddTransient<IEventConsumer, EventConsumer>();
            services.AddTransient<IApplicationEventPublisher, EventPublisher>();
            
            return services;
        }
    }
}