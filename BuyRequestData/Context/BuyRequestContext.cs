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

        private DbSet<Domain.Entities.BuyRequest> BuyRequests { get; set; }
        private DbSet<Domain.Entities.ProductRequest> BuyRequestProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BuyRequestConfiguration());
            modelBuilder.ApplyConfiguration(new ProductRequestConfiguration());
        }
    }
}