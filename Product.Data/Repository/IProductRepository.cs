using FinancialChallenge_Oscar.Infrastructure.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Product.Data.Repository
{
    public interface IProductRepository : IGenericRepository<Domain.Entities.Product>
    {
        Task<List<Domain.Entities.Product>> GetProductCategoryAsync(Expression<Func<Domain.Entities.Product, bool>> predicate);
    }
}