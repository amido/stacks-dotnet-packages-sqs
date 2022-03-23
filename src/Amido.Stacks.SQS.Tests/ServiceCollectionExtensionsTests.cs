// using System.ComponentModel.Design;
// using System.Threading.Tasks;
// using Amido.Stacks.Application.CQRS.Commands;
// using Amido.Stacks.SQS.Extensions;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Options;
// using Xunit;
//
// namespace Amido.Stacks.SQS.Tests;
//
// public class ServiceCollectionExtensionsTests
// {
// 	[Fact]
//     public async Task ServiceCollection_BuildsWithConfiguration_AsExpected()
//     {
//         /*
//          * 1.) Build the configuration
//          * 2.) Create ServiceCollection
//          * 3.) Bind option to the serviceCollection
//          * 4.) register stub service to serviceCollection
//          * 5.) get provider
//          * 6.) get service from provider
//          * 7.) assert the IOptionsMonitor is resolved
//          */
//         var builder = new ConfigurationBuilder()     
//             .AddJsonFile("appSettings.json");   
//         IConfiguration Configuration = builder.Build();
//
//         var options = Options.Create(Configuration);
// 	        
//         var services = new ServiceCollection();
//         services.Configure<AwsSqsConfiguration>(Configuration.GetSection("AwsSqsConfiguration"));
//         services.AddAwsSqs();
//         services.AddSingleton(options);
//
//         var provider = services.BuildServiceProvider();
// 		
// 		var dispatcher = provider.GetService<ICommandDispatcher>();
//
// 		var command = new MenuCommand(null, null);
//
// 		await dispatcher.SendAsync(command);
//     }
// }