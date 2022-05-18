using BuyRequest.Data.Configuration;
using FinancialChallenge_Oscar.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BuyRequest.Data.Context
{
    public class BuyRequestContext : DataContext
    {

        public BuyRequestContext(DbContextOptions<BuyRequestContext> options) : base(options)
        {

        }

        DbSet<Domain.Entities.BuyRequest> BuyRequests { get; set; }
        DbSet<Domain.Entities.ProductRequest> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BuyRequestConfiguration());
            modelBuilder.ApplyConfiguration(new ProductRequestConfiguration());
        }

    }
}
