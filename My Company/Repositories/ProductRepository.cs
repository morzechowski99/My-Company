using Microsoft.EntityFrameworkCore;
using My_Company.Areas.Warehouse.EnumTypes;
using My_Company.Areas.Warehouse.ViewModels;
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

        public async Task<IEnumerable<string>> GetCodesByPrefix(string prefix)
        {
            return await FindByCondition(p => p.EANCode.Contains(prefix))
                .Select(p => p.EANCode)
                .ToListAsync();
        }

        public IQueryable<Product> GetProductsByFilters(ProductsListFilters filters)
        {
            IQueryable<Product> query = FindAll()
                .Include(p => p.Supplier)
                .Include(p => p.Photos);

            query = Filter(query, filters);

            query = Sort(query, filters);

            return query;
        }

        private IQueryable<Product> Sort(IQueryable<Product> query, ProductsListFilters filters)
        {
            query = filters.SortOrder switch
            {
                var order when order == ProductsSortOrderEnum.EANASC => query.OrderBy(p => p.EANCode),
                var order when order == ProductsSortOrderEnum.EANDESC => query.OrderByDescending(p => p.EANCode),
                var order when order == ProductsSortOrderEnum.NameASC => query.OrderBy(p => p.Name),
                var order when order == ProductsSortOrderEnum.NameDESC => query.OrderByDescending(p => p.Name),
                _ => query
            };
            return query;
        }

        private IQueryable<Product> Filter(IQueryable<Product> query, ProductsListFilters filters)
        {
            if (!string.IsNullOrEmpty(filters.SearchString))
            {
                var ss = filters.SearchString.ToLower();
                query = query.Where(p => ss.Contains(p.Name.ToLower()) || p.Name.ToLower().Contains(ss)
                    || p.EANCode.Contains(ss) || ss.Contains(p.EANCode) ||
                    p.ProductCategories.Any(pc => pc.Category.CategoryName.ToLower().Contains(ss) ||
                    ss.Contains(pc.Category.CategoryName.ToLower())));
            }
            if (filters.Categories != null && filters.Categories.Any())
            {
                query = query
                    .Where(p => p.ProductCategories
                        .Any(pc => filters.Categories
                        .Contains(pc.CategoryId)));
            }
            if (filters.Eans != null && filters.Eans.Any())
            {
                query = query.Where(p => filters.Eans.Contains(p.EANCode));
            }
            if (filters.States != null && filters.States.Any() && filters.States.Count < 3)
            {
                if (filters.States.Count == 1)
                {
                    query = filters.States.FirstOrDefault() switch
                    {
                        var s when s == StockState.Good => query.Where(p => p.MagazineCount > p.Demand * 1.15),
                        var s when s == StockState.RunningOut => query.Where(p => p.MagazineCount <= p.Demand * 1.15 && p.MagazineCount > p.Demand),
                        var s when s == StockState.Critical => query.Where(p => p.MagazineCount <= p.Demand),
                        _ => query
                    };
                }
                else
                {
                    if(filters.States.Contains(StockState.Critical) && filters.States.Contains(StockState.Good))
                    {
                        query = query.Where(p => p.MagazineCount > p.Demand * 1.15 || p.MagazineCount < p.Demand);
                    }
                    else if (filters.States.Contains(StockState.Good) && filters.States.Contains(StockState.RunningOut)) 
                    {
                        query = query.Where(p => p.MagazineCount > p.Demand );
                    }
                    else
                    {
                        query = query.Where(p => p.MagazineCount <= p.Demand * 1.15);
                    }
                }
            }
            if(filters.Statuses != null && filters.Statuses.Any())
            {
                query = query.Where(p => filters.Statuses.Contains(p.Status));
            }
            if(filters.Suppliers != null && filters.Suppliers.Any())
            {
                query = query.Where(p => filters.Suppliers.Contains(p.SupplierId));
            }

            return query;
        }
    }
}
