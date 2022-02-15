//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using Microsoft.EntityFrameworkCore;
using My_Company.Data;
using My_Company.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace My_Company.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        protected ApplicationDbContext Context { get { return _context; } }
        public RepositoryBase(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Create(T entity)
        {
            _context.Add(entity);
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);
        }

        public IQueryable<T> FindAll()
        {
            return _context.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression).AsNoTracking();
        }

        public void Update(T entity)
        {
            _context.Update(entity);
        }

        public async Task<bool> Exists(params object[] values)
        {
            var entity = await _context.Set<T>().FindAsync(values);

            return entity == null ? false : true;
        }

        public IQueryable<T> GetTracked()
        {
            return _context.Set<T>().AsTracking();
        }

        public async Task<T> GetOne(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(expression);
        }
    }
}
