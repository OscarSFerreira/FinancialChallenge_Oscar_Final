using FinancialChallenge_Oscar.Infrastructure.BaseClass;
using FinancialChallenge_Oscar.Infrastructure.Context;
using FinancialChallenge_Oscar.Infrastructure.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FinancialChallenge_Oscar.Infrastructure.Repository.Generic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : EntityBase
    {
        private readonly DataContext _context;
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
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsNoTracking();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<T>> GetAllWithPaging(PageParameter page)        //testar no geral
        {
            var query = _dbSet
                .Skip((page.PageNumber - 1) * page.PageSize)
                .Take(page.PageSize);

            if (_include != null)
            {
                query = _include(query);
            }
            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var query = _dbSet
                .Where(e => e.Id == id);

            if (_include != null)
                query = _include(query);
            var result = await query.AsNoTracking().FirstOrDefaultAsync();

            return result;
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            var query = _dbSet
                .Where(predicate);

            if (_include != null)
                query = _include(query);

            var result = await query.AsNoTracking().FirstOrDefaultAsync();

            return result;
        }
    }
}