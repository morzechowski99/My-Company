using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace My_Company.Interfaces
{
    public interface IDatabaseView<T>
    {
        IQueryable<T> GetData();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        Task<T> GetOne(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> GetAll();
    }
}
