using FinancialChallenge_Oscar.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Product.Data.Context
{
    public class ProductContext : DataContext
    {

        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {

        }

        DbSet<Domain.Entities.Product> ProductsList { get; set; }

    }
}
