using My_Company.Data;
using My_Company.Interfaces;
using My_Company.Models;

namespace My_Company.Repositories
{
    public class ProductAttributesRepository : RepositoryBase<ProductAttribute>, IProductAttributeRepository
    {
        public ProductAttributesRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
