using BuyRequest.Data.Context;
using FinancialChallenge_Oscar.Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BuyRequest.Data.Repository.ProductRequest
{
    public class ProductRequestRepository : GenericRepository<Domain.Entities.ProductRequest>, IProductRequestRepository
    {

        private readonly BuyRequestContext _context;

        public ProductRequestRepository(BuyRequestContext context) : base(context)
        {
            _context = context;
        }

        //public async Task<Domain.Entities.ProductRequest> GetByIdAsync(Guid id)
        //{
        //    return await _context.Set<Domain.Entities.ProductRequest>()
        //        .AsNoTracking()
        //        .FirstOrDefaultAsync(e => e.Id == id);
        //}

        //public IQueryable<Domain.Entities.ProductRequest> GetAllByRequestId(Guid requestId)
        //{
        //    return _context.Set<Domain.Entities.ProductRequest>()
        //        .AsNoTracking()
        //        .Where(e => e.BuyRequestId == requestId);
        //}

    }

}
