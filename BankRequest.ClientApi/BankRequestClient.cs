using BankRequest.Application.DTO;
using BankRequest.ClientApi.Interfaces;
using BankRequest.ClientApi.Options;
using BankRequest.Domain.Entities.Enum;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BankRequest.ClientApi
{
    public class BankRequestClient : IBankRequestClient
    {

        private readonly HttpClient _client;
        private readonly BankRequestOptions _options;

        public BankRequestClient(IOptions<BankRequestOptions> options, HttpClient client)
        {
            _options = options.Value;
            _client = client;
        }

        public async Task<bool> PostCashBank(Origin origin, Guid id, string description, Domain.Entities.Enum.Type type, decimal amount)
        {

            var opt = _options.GetBankRequestEndPoint();

            BankRequestDTO bankRequest = new BankRequestDTO()
            {
                Origin = origin,
                OriginId = id,
                Description = description,
                Type = type,
                Amount = amount
            };

            var response = await _client.PostAsJsonAsync(opt, bankRequest);

            if (!response.IsSuccessStatusCode)
            {
                var error = response.Content.ToString();
                throw new Exception(error);
            }

            return response != null && response.IsSuccessStatusCode;
        }
    }
}
