using FinancialChallenge_Oscar.Infrastructure.ErrorMessage;
using FinancialChallenge_Oscar.Infrastructure.Paging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialChallenge_Oscar.Infrastructure.Repository.Generic
{
    public interface IGenericRepository<T> where T : class
    {
        Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        IQueryable<T> GetAll();

        IEnumerable<T> GetAllWithPaging(PageParameter page);

        ErrorMessage<T> BadRequestMessage(T entity, string msg);

        ErrorMessage<T> NotFoundMessage(T entity);
    }
}
