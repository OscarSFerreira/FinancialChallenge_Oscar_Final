using BuyRequest.Data.Context;
using FinancialChallenge_Oscar.Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BuyRequest.Data.Repository.BuyRequest
{
    public class BuyRequestRepository : GenericRepository<Domain.Entities.BuyRequest>, IBuyRequestRepository
    {

        private readonly BuyRequestContext _context;

        public BuyRequestRepository(BuyRequestContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Domain.Entities.BuyRequest> GetByIdAsync(Guid id)
        {
            return await _context.Set<Domain.Entities.BuyRequest>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Domain.Entities.BuyRequest> GetByClientIdAsync(Guid clientId)
        {
            return await _context.Set<Domain.Entities.BuyRequest>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.ClientId == clientId);
        }

    }
}
