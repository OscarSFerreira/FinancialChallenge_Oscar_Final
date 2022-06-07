using FinancialChallenge_Oscar.Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using Product.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Product.Data.Repository
{
    public class ProductRepository : GenericRepository<Domain.Entities.Product>, IProductRepository
    {
        public ProductRepository(ProductContext context) : base(context)
        {
        }

        public async Task<List<Domain.Entities.Product>> GetProductCategoryAsync(Expression<Func<Domain.Entities.Product, bool>> predicate)
        {
            var query = _dbSet
                .Where(predicate);

            if (_include != null)
                query = _include(query);

            var result = await query.AsNoTracking().ToListAsync();

            return result;
        }
    }
}