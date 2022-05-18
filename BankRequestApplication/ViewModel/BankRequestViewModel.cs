using System.Collections.Generic;

namespace BankRequest.Application.ViewModel
{
    public class BankRequestViewModel
    {

        public List<Domain.Entities.BankRequest> BankRecords { get; set; }

        public decimal Total { get; set; }

    }
}
