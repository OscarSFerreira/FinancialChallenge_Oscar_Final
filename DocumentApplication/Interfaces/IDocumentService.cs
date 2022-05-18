using Document.Application.DTO;
using FinancialChallenge_Oscar.Infrastructure.Paging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Document.Application.Interfaces
{
    public interface IDocumentService
    {

        Task<Domain.Entities.Document> Post(DocumentDTO input);
        Task<List<Domain.Entities.Document>> GetAll(PageParameter parameters);
        Task<Domain.Entities.Document> GetById(Guid id);
        Task<Domain.Entities.Document> ChangeDocument(Guid id, DocumentDTO input);
        Task<Domain.Entities.Document> ChangeState(Guid id, bool Status);
        Task<Domain.Entities.Document> DeleteById(Guid id);

    }
}
