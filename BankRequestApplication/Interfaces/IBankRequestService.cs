using BankRequest.Application.DTO;
using BankRequest.Application.ViewModel;
using FinancialChallenge_Oscar.Infrastructure.Paging;
using System;
using System.Threading.Tasks;

namespace BankRequest.Application.Interfaces
{
    public interface IBankRequestService
    {

        Task PostBankRecord(BankRequestDTO input);
        Task<BankRequestViewModel> GetAll(PageParameter parameters);
        Task<Domain.Entities.BankRequest> GetById(Guid id);
        Task<Domain.Entities.BankRequest> GetByOriginId(Guid OriginId);
        Task<Domain.Entities.BankRequest> ChangeBankRequest(Guid id, BankRequestDTO bankRequest);
    }
}
