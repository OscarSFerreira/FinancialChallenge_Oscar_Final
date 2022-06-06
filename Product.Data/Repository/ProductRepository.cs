using FinancialChallenge_Oscar.Infrastructure.Repository.Generic;
using Product.Data.Context;

namespace Product.Data.Repository
{
    public class ProductRepository : GenericRepository<Domain.Entities.Product>, IProductRepository
    {
        public ProductRepository(ProductContext context) : base(context)
        {
        }
    }
}