using System;

namespace FinancialChallenge_Oscar.Infrastructure.RabbitMQ.Generic.Options
{
    public class BankRequestOptions
    {
        private string _baseAddress;

        public string BaseAddress
        {
            get
            {
                return _baseAddress ?? throw new InvalidOperationException("Base address Financial API must be setted.");
            }
            set { _baseAddress = value; }
        }

        private string _endPoint;

        public string EndPoint
        {
            get
            {
                return _endPoint ?? throw new InvalidOperationException("Bank Request EndPoint must be setted.");
            }
            set { _endPoint = value; }
        }

        private string _hostName;

        public string HostName
        {
            get
            {
                return _hostName ?? throw new InvalidOperationException("Bank Request HostName must be setted.");
            }
            set { _hostName = value; }
        }

        private string _queueName;

        public string QueueName
        {
            get
            {
                return _queueName ?? throw new InvalidOperationException("Bank Request QueueName must be setted.");
            }
            set { _queueName = value; }
        }

        public string GetBankRequestEndPoint() => $"{BaseAddress}/{EndPoint}";

        public string GetBankRequestHostName() => $"{HostName}";

        public string GetBankRequestQueueName() => $"{QueueName}";
    }
}