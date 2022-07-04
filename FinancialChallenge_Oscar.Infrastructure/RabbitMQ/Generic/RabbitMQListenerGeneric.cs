using FinancialChallenge_Oscar.Infrastructure.RabbitMQ.Generic.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FinancialChallenge_Oscar.Infrastructure.RabbitMQ.Generic
{
    public class RabbitMQListenerGeneric : IHostedService
    {
        private readonly BankRequestOptions _options;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string queueName;
        private readonly string hostName;

        public RabbitMQListenerGeneric(IOptions<BankRequestOptions> options)
        {
            _options = options.Value;
            queueName = _options.GetBankRequestQueueName();
            hostName = _options.GetBankRequestHostName();

            var factory = new ConnectionFactory
            {
                HostName = hostName,
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await MessageListener();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task MessageListener()
        {
            _channel.QueueDeclare(queueName, exclusive: false);
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var result = await SendMessage(message);
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _channel.BasicNack(ea.DeliveryTag, false, true);
                }

                //T record = (T)Newtonsoft.Json.JsonConvert.DeserializeObject<T>(message);
                //_bankRequestService.PostBankRecord(record);
            };

            _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
        }

        public virtual async Task<bool> SendMessage(string message)
        {
            throw new NotImplementedException();
        }
    }
}