using FinancialChallenge_Oscar.Infrastructure.BaseClass;
using FinancialChallenge_Oscar.Infrastructure.Context;
using FinancialChallenge_Oscar.Infrastructure.ErrorMessage;
using FinancialChallenge_Oscar.Infrastructure.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace FinancialChallenge_Oscar.Infrastructure.Repository.Generic
{

    public class GenericRepository<T> : IGenericRepository<T> where T : EntityBase
    {
        private readonly DataContext _context;
        public List<string> errorList = new List<string>();
        protected readonly DbSet<T> _dbSet;
        protected Func<IQueryable<T>, IIncludableQueryable<T, object>> _include;

        public GenericRepository(DataContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual void SetInclude(Func<IQueryable<T>, IIncludableQueryable<T, object>> include)
        {
            _include = include;
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
        private async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        //public async Task<bool> ExistAsync(Guid id)
        //{
        //    return await _context.Set<T>().AnyAsync(x => x.Id == id);
        //}
        public async Task<List<T>> GetAllWithPaging(PageParameter page)        //testar no geral
        {
            var query = _dbSet
                .Skip((page.PageNumber - 1) * page.PageSize)
                .Take(page.PageSize)
                .AsNoTracking();

            if (_include != null)
            {
                query = _include(query);
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var query = _dbSet
                .Where(e => e.Id == id)
                .AsNoTracking();

            if (_include != null)
            {
                query = _include(query);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            var query = _dbSet
                .Where(predicate);

            if (_include != null)
            {
                query = _include(query);
            }
            return await query.FirstOrDefaultAsync();
        }

        public ErrorMessage<T> BadRequestMessage(T entity, string msg)
        {
            errorList.Add(msg);
            var error = new ErrorMessage<T>(HttpStatusCode.BadRequest.GetHashCode().ToString(), errorList, entity);
            return error;
        }

        public ErrorMessage<T> NotFoundMessage(T entity)
        {
            errorList.Add("This database does not contain the data you requested!");
            var error = new ErrorMessage<T>(HttpStatusCode.NotFound.GetHashCode().ToString(), errorList, entity);
            return error;
        }

    }

}
