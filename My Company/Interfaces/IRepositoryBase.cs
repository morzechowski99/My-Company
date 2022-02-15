//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace My_Company.Interfaces
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        IQueryable<T> GetTracked();
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> Exists(params object[] values);
        Task<T> GetOne(Expression<Func<T, bool>> expression);
    }
}
