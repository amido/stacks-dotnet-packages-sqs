using Amazon.SQS;
using Microsoft.Extensions.DependencyInjection;

namespace Amido.Stacks.SQS.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAwsSqs(this IServiceCollection services)
        {
            services.AddAWSService<IAmazonSQS>();
            return services;
        }
    }
}