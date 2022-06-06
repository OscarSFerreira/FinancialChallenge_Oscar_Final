using BankRequest.Application.DTO;
using BankRequest.Application.Model;
using FinancialChallenge_Oscar.Infrastructure.Paging;
using System;
using System.Threading.Tasks;

namespace BankRequest.Application.Interfaces
{
    public interface IBankRequestService
    {
        Task PostBankRecord(BankRequestDTO input);

        Task<BankRequestModel> GetAll(PageParameter parameters);

        Task<Domain.Entities.BankRequest> GetById(Guid id);

        Task<Domain.Entities.BankRequest> GetByOriginId(Guid OriginId);

        Task<Domain.Entities.BankRequest> ChangeBankRequest(Guid id, BankRequestDTO bankRequest);
    }
}