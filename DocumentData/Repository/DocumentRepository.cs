using Document.Data.Context;
using FinancialChallenge_Oscar.Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Document.Data.Repository
{

    public class DocumentRepository : GenericRepository<Domain.Entities.Document>, IDocumentRepository
    {
        private readonly DocumentDataContext _context;

        public DocumentRepository(DocumentDataContext context) : base(context)
        {

            _context = context;

        }

        public async Task<Domain.Entities.Document> GetByIdAsync(Guid id)
        {
            return await _context.Set<Domain.Entities.Document>()
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(e => e.Id == id);
        }
    }

}
