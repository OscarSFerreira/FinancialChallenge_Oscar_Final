using BuyRequest.Data.Context;
using FinancialChallenge_Oscar.Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace BuyRequest.Data.Repository.BuyRequest
{
    public class BuyRequestRepository : GenericRepository<Domain.Entities.BuyRequest>, IBuyRequestRepository
    {

        private readonly BuyRequestContext _context;

        public BuyRequestRepository(BuyRequestContext context) : base(context)
        {
            _context = context;
            SetInclude(x => x.Include(i => i.Products));
        }

    }
}
