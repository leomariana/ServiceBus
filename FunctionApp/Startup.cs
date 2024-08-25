using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Azure.Messaging.ServiceBus;
using System;

[assembly: FunctionsStartup(typeof(FunctionApp.StartUp))]

namespace FunctionApp
{
    public class StartUp : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton(serviceProvider =>
            {
                string connectionString = Environment.GetEnvironmentVariable("ServiceBusConnectionString");
                return new ServiceBusClient(connectionString);
            });
        }
    }

}