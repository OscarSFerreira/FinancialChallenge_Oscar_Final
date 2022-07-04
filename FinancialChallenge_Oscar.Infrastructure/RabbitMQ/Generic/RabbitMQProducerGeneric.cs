using FinancialChallenge_Oscar.Infrastructure.RabbitMQ.Generic.Interfaces;
using FinancialChallenge_Oscar.Infrastructure.RabbitMQ.Generic.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace FinancialChallenge_Oscar.Infrastructure.RabbitMQ.Generic
{
    public class RabbitMQProducerGeneric : IMessageProducer
    {
        private readonly BankRequestOptions _bankRequestOptions;

        public RabbitMQProducerGeneric(IOptions<BankRequestOptions> bankRequestOptions)
        {
            _bankRequestOptions = bankRequestOptions.Value;
        }

        public void PublishMessage<T>(T message, string queueName)
        {
            var factory = new ConnectionFactory
            {
                HostName = _bankRequestOptions.GetBankRequestHostName(),
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queueName, exclusive: false);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
        }
    }
}