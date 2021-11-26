using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Interfaces
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        Task<bool> CheckEANExists(string EANCode, int id);
        IQueryable<Product> GetProductsByFilters(ProductsListFilters filters);
        Task<IEnumerable<string>> GetCodesByPrefix(string prefix);
        Task<Product> GetProductById(int id);
        Task<Product> GetProductWithCategoriesAndAttributesByIdTracked(int id);
        Task<Product> GetProductWithoutVirtualPropertiesById(int id);
        Task<IEnumerable<ProductAttribute>> GetAttributesByProductId(int id);
        Task<Product> GetProductByEANCode(string ean);
        Task<IEnumerable<Product>> SearchProductByQueryStringWithoutArchived(string query);
        IQueryable<Product> GetByFilters(My_Company.Areas.Shop.ViewModels.Products.ProductsListFilters filters);
        Task<List<Product>> GetProductsByCategoryId(int value);
        Task<Product> GetProductDetailsById(int id);
        Task<List<Product>> GetCardItems(List<int> list);
        Task<bool> CheckProductsActive(List<int> productsIds);
    }
}
