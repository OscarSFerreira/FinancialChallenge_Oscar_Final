using BankRequest.ClientApi.Interfaces;
using BankRequest.ClientApi.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BankRequest.ClientApi.Configuration
{
    public static class BankRequestConfiguration
    {

        public static void AddBankRequestConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<BankRequestOptions>(options =>
            {
                options.BaseAddress = configuration["BankRequest.ClientApi:BaseAddress"];
                options.EndPoint = configuration["BankRequest.ClientApi:EndPoint"];
            });
            services.AddHttpClient<IBankRequestClient, BankRequestClient>();

        }

    }
}
