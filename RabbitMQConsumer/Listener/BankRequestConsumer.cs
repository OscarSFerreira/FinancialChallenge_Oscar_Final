using BankRequest.Application.DTO;
using BankRequest.Application.Interfaces;
using FinancialChallenge_Oscar.Infrastructure.RabbitMQ.Generic;
using FinancialChallenge_Oscar.Infrastructure.RabbitMQ.Generic.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace RabbitMQConsumer.Listener
{
    public class BankRequestConsumer : RabbitMQListenerGeneric
    {
        private readonly IServiceProvider _serviceProvider;

        public BankRequestConsumer(IOptions<BankRequestOptions> options, IServiceProvider serviceProvider) : base(options)
        {
            _serviceProvider = serviceProvider;
        }

        public override async Task<bool> SendMessage(string message)
        {
            try
            {
                BankRequestDTO record = (BankRequestDTO)Newtonsoft.Json.JsonConvert
                                .DeserializeObject<BankRequestDTO>(message);

                if (record != null)
                {
                    using var scope = _serviceProvider.CreateScope();
                    var _bankRequestService = scope.ServiceProvider.GetService<IBankRequestService>();
                    await _bankRequestService.PostBankRecord(record);
                }
                else
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}