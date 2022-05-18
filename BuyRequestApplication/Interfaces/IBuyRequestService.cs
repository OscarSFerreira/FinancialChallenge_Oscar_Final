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
        Task<IEnumerable<Domain.Entities.BuyRequest>> GetAll(PageParameter parameters);
        Task<Domain.Entities.BuyRequest> GetById(Guid id);
        Task<Domain.Entities.BuyRequest> GetByClientIdAsync(Guid clientId);
        Task<Domain.Entities.BuyRequest> UpdateAsync(BuyRequestDTO buyinput);
        Task<Domain.Entities.BuyRequest> ChangeState(Guid id, Status state);
        Task<Domain.Entities.BuyRequest> DeleteById(Guid id);

    }
}
