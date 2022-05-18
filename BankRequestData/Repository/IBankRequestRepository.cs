using FinancialChallenge_Oscar.Infrastructure.Repository.Generic;
using System;
using System.Threading.Tasks;

namespace BankRequest.Data.Repository
{
    public interface IBankRequestRepository : IGenericRepository<Domain.Entities.BankRequest>
    {

        Task<Domain.Entities.BankRequest> GetByIdAsync(Guid id);

        //Task<HttpResponseMessage> CreateBankRecord(Origin origin, Guid originId, string description, Domain.Entities.Type type, decimal amount);
        Task<Domain.Entities.BankRequest> GetByOriginIdAsync(Guid OriginId);

    }
}
