using System;

namespace BankRequest.ClientApi.Options
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
        public string GetBankRequestEndPoint() => $"{BaseAddress}/{EndPoint}";

    }
}
