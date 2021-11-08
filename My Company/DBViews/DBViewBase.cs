using Microsoft.EntityFrameworkCore;
using My_Company.Data;
using My_Company.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace My_Company.DBViews
{
    public class DBViewBase<T> : IDatabaseView<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public DBViewBase(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>()
                .AsNoTracking()
                .Where(expression);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await GetData().ToListAsync();
        }

        public IQueryable<T> GetData()
        {
            return _context.Set<T>().AsNoTracking();
        }

        public async Task<T> GetOne(Expression<Func<T, bool>> expression)
        {
            return await FindByCondition(expression).FirstOrDefaultAsync();
        }
    }
}
