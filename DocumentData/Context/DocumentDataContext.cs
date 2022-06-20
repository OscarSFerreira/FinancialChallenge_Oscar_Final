using FinancialChallenge_Oscar.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Document.Data.Context
{
    public class DocumentDataContext : DataContext
    {
        public DocumentDataContext(DbContextOptions<DocumentDataContext> options) : base(options)
        {
        }

        public DbSet<Domain.Entities.Document> Documents { get; set; }
    }
}