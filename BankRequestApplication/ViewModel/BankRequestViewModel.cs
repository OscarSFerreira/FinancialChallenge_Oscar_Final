using System.Collections.Generic;

namespace BankRequest.Application.ViewModel
{
    public class BankRequestViewModel
    {

        public IEnumerable<Domain.Entities.BankRequest> BankRecords { get; set; }

        public decimal Total { get; set; }

    }
}
