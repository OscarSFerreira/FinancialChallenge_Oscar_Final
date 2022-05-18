using FinancialChallenge_Oscar.Infrastructure.Repository.Generic;
using System;
using System.Threading.Tasks;

namespace BuyRequest.Data.Repository.BuyRequest
{
    public interface IBuyRequestRepository : IGenericRepository<Domain.Entities.BuyRequest>
    {

        Task<Domain.Entities.BuyRequest> GetByIdAsync(Guid id);

        Task<Domain.Entities.BuyRequest> GetByClientIdAsync(Guid clientId);


    }
}
