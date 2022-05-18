using FinancialChallenge_Oscar.Infrastructure.Repository.Generic;
using System;
using System.Threading.Tasks;

namespace Document.Data.Repository
{
    public interface IDocumentRepository : IGenericRepository<Domain.Entities.Document>
    {

        Task<Domain.Entities.Document> GetByIdAsync(Guid id);

    }
}
