using My_Company.Data;
using My_Company.Interfaces;
using My_Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Repositories
{
    public class ProductAttributesRepository : RepositoryBase<ProductAttribute>, IProductAttributeRepository
    {
        public ProductAttributesRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
