using BankRequest.Data.Context;
using FinancialChallenge_Oscar.Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BankRequest.Data.Repository
{
    public class BankRequestRepository : GenericRepository<Domain.Entities.BankRequest>, IBankRequestRepository
    {

        private readonly BankRequestContext _context;

        public BankRequestRepository(BankRequestContext context) : base(context)
        {
            _context = context;
        }

        //public async Task<Domain.Entities.BankRequest> GetByIdAsync(Guid id)
        //{
        //    return await _context.Set<Domain.Entities.BankRequest>()
        //        .AsNoTracking()
        //        .FirstOrDefaultAsync(e => e.Id == id);
        //}

        //public async Task<Domain.Entities.BankRequest> GetByOriginIdAsync(Guid OriginId)
        //{
        //    return await _context.Set<Domain.Entities.BankRequest>()
        //        .AsNoTracking()
        //        .FirstOrDefaultAsync(e => e.OriginId == OriginId);
        //}

    }
}
