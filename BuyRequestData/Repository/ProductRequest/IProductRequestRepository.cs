using BuyRequest.Domain.Entities.Enum;
using FinancialChallenge_Oscar.Infrastructure.Repository.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BuyRequest.Data.Repository.ProductRequest
{
    public interface IProductRequestRepository : IGenericRepository<Domain.Entities.ProductRequest>
    {

        //Task<Domain.Entities.ProductRequest> GetByIdAsync(Guid id);
        //IQueryable<Domain.Entities.ProductRequest> GetAllByRequestId(Guid requestId);

    }
}
