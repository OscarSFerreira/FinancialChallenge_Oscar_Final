using Microsoft.EntityFrameworkCore;

namespace FinancialChallenge_Oscar.Infrastructure.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
    }
}