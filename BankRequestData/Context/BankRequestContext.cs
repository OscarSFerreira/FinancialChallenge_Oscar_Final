using FinancialChallenge_Oscar.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BankRequest.Data.Context
{
    public class BankRequestContext : DataContext
    {

        public BankRequestContext(DbContextOptions<BankRequestContext> options) : base(options)
        {

        }

        public DbSet<Domain.Entities.BankRequest> BankRequests { get; set; }

    }

}
