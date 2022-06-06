using BuyRequest.Data.Context;
using FinancialChallenge_Oscar.Infrastructure.Repository.Generic;

namespace BuyRequest.Data.Repository.ProductRequest
{
    public class ProductRequestRepository : GenericRepository<Domain.Entities.ProductRequest>, IProductRequestRepository
    {
        public ProductRequestRepository(BuyRequestContext context) : base(context)
        {
        }
    }
}