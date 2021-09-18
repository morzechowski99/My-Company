using My_Company.Models;
using My_Company.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Interfaces
{
    public interface ICategoriesRepository : IRepositoryBase<Category>
    {
        Task<IEnumerable<Category>> GetAll();
        Task<IEnumerable<CategoryTree>> GetCategoriesTree();
        Task<IEnumerable<Models.Attribute>> GetAllAttributesByCategoryId(int? categoryId);
        Task<bool> CheckName(string categoryName);
    }
}
