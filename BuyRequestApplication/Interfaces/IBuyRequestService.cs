using BuyRequest.Application.DTO;
using BuyRequest.Domain.Entities.Enum;
using FinancialChallenge_Oscar.Infrastructure.Paging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuyRequest.Application.Interfaces
{
    public interface IBuyRequestService
    {

        Task<Domain.Entities.BuyRequest> Post(BuyRequestDTO buyinput);
        Task<List<Domain.Entities.BuyRequest>> GetAll(PageParameter parameters);
        Task<Domain.Entities.BuyRequest> GetById(Guid id);
        Task<Domain.Entities.BuyRequest> GetByClientIdAsync(Guid clientId);
        Task<Domain.Entities.BuyRequest> UpdateAsync(Guid id, BuyRequestDTO buyinput);
        Task<Domain.Entities.BuyRequest> ChangeState(Guid id, Status state);
        Task<Domain.Entities.BuyRequest> DeleteById(Guid id);

    }
}
