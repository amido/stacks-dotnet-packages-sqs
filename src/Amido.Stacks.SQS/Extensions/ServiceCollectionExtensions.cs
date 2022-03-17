using Amazon.SQS;
using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Amido.Stacks.SQS.Consumer;
using Amido.Stacks.SQS.Publisher;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Amido.Stacks.SQS.Extensions
{
    public class ServiceCollectionExtensions
    {
        private IConfiguration Configuration { get; }
        
        public ServiceCollectionExtensions(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public IServiceCollection AddAwsSqs(IServiceCollection services)
        {
            // This will get the config options from appsettings under "AWS" and wrap it with AWSOption model
            var awsOptions = Configuration.GetAWSOptions();
            services.AddDefaultAWSOptions(awsOptions);
            services.AddAWSService<IAmazonSQS>(awsOptions);  
            services.AddTransient<IEventConsumer, EventConsumer>();
            services.AddTransient<IApplicationEventPublisher, EventPublisher>();
            
            return services;
        }
    }
}