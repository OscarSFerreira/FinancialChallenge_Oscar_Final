using System.Collections.Generic;

namespace BankRequest.Application.Model
{
    public class BankRequestModel
    {

        public IEnumerable<Domain.Entities.BankRequest> BankRecords { get; set; }

        public decimal Total { get; set; }

    }
}
