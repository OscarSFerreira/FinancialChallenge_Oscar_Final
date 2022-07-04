using FinancialChallenge_Oscar.Infrastructure.RabbitMQ.Generic.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialChallenge_Oscar.Infrastructure.RabbitMQ.Generic.Configuration
{
    public static class BankRequestConfiguration
    {
        public static void AddBankRequestConfigurationMQ(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BankRequestOptions>(options =>
            {
                options.BaseAddress = configuration["BankRequestClientApi:BaseAddress"];
                options.EndPoint = configuration["BankRequestClientApi:EndPoint"];
                options.HostName = configuration["BankRequestClientApi:HostName"];
                options.QueueName = configuration["BankRequestClientApi:QueueName"];
            });
            services.AddSingleton<RabbitMQListenerGeneric, RabbitMQListenerGeneric>();
        }
    }
}