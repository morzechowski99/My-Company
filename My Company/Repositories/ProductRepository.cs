using Microsoft.EntityFrameworkCore;
using My_Company.Data;
using My_Company.Interfaces;
using My_Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> CheckEANExists(string EANCode)
        {
            return await FindAll().AnyAsync(p => p.EANCode == EANCode);
        }

        public new void Create(Product p)
        {
            p.ProductCategories = p.ProductCategories.Where(pc => pc.CategoryId != 0).ToList();
            base.Create(p);
        }
    }
}
