using FinancialChallenge_Oscar.Infrastructure.Repository.Generic;

namespace Document.Data.Repository
{
    public interface IDocumentRepository : IGenericRepository<Domain.Entities.Document>
    {

        //Task<Domain.Entities.Document> GetByIdAsync(Guid id);

    }
}
