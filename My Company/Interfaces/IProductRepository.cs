using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Interfaces
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task<bool> CheckEANExists(string EANCode);
        IQueryable<Product> GetProductsByFilters(ProductsListFilters filters);
        Task<IEnumerable<string>> GetCodesByPrefix(string prefix);
        Task<Product> GetProductById(int id);
    }
}
