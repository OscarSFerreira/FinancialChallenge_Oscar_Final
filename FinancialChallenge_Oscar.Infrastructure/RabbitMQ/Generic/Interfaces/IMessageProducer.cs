namespace FinancialChallenge_Oscar.Infrastructure.RabbitMQ.Generic.Interfaces
{
    public interface IMessageProducer
    {
        void PublishMessage<T>(T message, string queueName);
    }
}