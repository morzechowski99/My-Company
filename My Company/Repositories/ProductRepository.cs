using Microsoft.EntityFrameworkCore;
using My_Company.Areas.Shop.Enums;
using My_Company.Areas.Warehouse.EnumTypes;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Data;
using My_Company.EnumTypes;
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

        public async Task<bool> CheckEANExists(string EANCode, int id)
        {
            return await FindAll().AnyAsync(p => p.EANCode == EANCode && p.Id != id);
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
                    if (filters.States.Contains(StockState.Critical) && filters.States.Contains(StockState.Good))
                    {
                        query = query.Where(p => p.MagazineCount > p.Demand * 1.15 || p.MagazineCount < p.Demand);
                    }
                    else if (filters.States.Contains(StockState.Good) && filters.States.Contains(StockState.RunningOut))
                    {
                        query = query.Where(p => p.MagazineCount > p.Demand);
                    }
                    else
                    {
                        query = query.Where(p => p.MagazineCount <= p.Demand * 1.15);
                    }
                }
            }
            if (filters.Statuses != null && filters.Statuses.Any())
            {
                query = query.Where(p => filters.Statuses.Contains(p.Status));
            }
            if (filters.Suppliers != null && filters.Suppliers.Any())
            {
                query = query.Where(p => filters.Suppliers.Contains(p.SupplierId));
            }

            return query;
        }

        public async Task<Product> GetProductById(int id)
        {
            return await FindByCondition(p => p.Id == id)
                .Include(p => p.VATRate)
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .Include(p => p.Supplier)
                .Include(p => p.ProductAttributes)
                .ThenInclude(p => p.Attribute)
                .Include(p => p.Photos)
                .FirstOrDefaultAsync();
        } 
        
        public async Task<Product> GetProductDetailsById(int id)
        {
            return await FindByCondition(p => p.Id == id)
                .Include(p => p.VATRate)
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .Include(p => p.ProductAttributes)
                .ThenInclude(p => p.Attribute)
                .Include(p => p.Photos.Where(p => !p.IsListPhoto).OrderByDescending(p => p.IsMainPhoto))
                .FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductWithCategoriesAndAttributesByIdTracked(int id)
        {
            return await GetTracked()
                .Include(p => p.ProductCategories)
                .Include(p => p.ProductAttributes)
                .ThenInclude(pa => pa.Attribute)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> GetProductWithoutVirtualPropertiesById(int id)
        {
            return await GetOne(p => p.Id == id);
        }

        public async Task<IEnumerable<ProductAttribute>> GetAttributesByProductId(int id)
        {
            var product = await FindByCondition(p => p.Id == id)
                .Include(p => p.ProductAttributes)
                .ThenInclude(pa => pa.Attribute)
                .ThenInclude(a => a.AttributeDictionaryValues)
                .FirstOrDefaultAsync();

            return product == null ? new List<ProductAttribute>() : product.ProductAttributes;

        }

        public async Task<Product> GetProductByEANCode(string ean)
        {
            return await FindByCondition(p => p.EANCode == ean)
                .Include(p => p.Photos)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> SearchProductByQueryStringWithoutArchived(string query)
        {
            query = query.ToLower();
            return await FindByCondition(p => (query.Contains(p.Name.ToLower()) || p.Name.ToLower().Contains(query)
                || p.EANCode.Contains(query) || query.Contains(p.EANCode)) && p.Status != ProductStatus.Archived).ToListAsync();
        }

        public IQueryable<Product> GetByFilters(My_Company.Areas.Shop.ViewModels.Products.ProductsListFilters filters)
        {
            IQueryable<Product> products = FindByCondition(p => p.Status == ProductStatus.Active)
                                                .Include(p => p.Photos)
                                                .Include(p => p.VATRate)
                                                .Include(p => p.ProductCategories.Where(pc => pc.IsProductCategory))
                                                .ThenInclude(pc => pc.Category);

            if (filters.CategoryId != null)
                products = products.Where(p => p.ProductCategories.Any(po => po.CategoryId == filters.CategoryId));

            products = Filter(products, filters);
            products = Sort(products, filters.SortOrder);

            return products;
        }

        private IQueryable<Product> Filter(IQueryable<Product> products, Areas.Shop.ViewModels.Products.ProductsListFilters filters)
        {
            if (filters.PriceFrom.HasValue)
            {
                products = products.Where(p => (p.NettoPrice / 100.0M + p.NettoPrice / 100.0M * (p.VATRate.Rate / 100.0M) >= filters.PriceFrom));
            }
            if (filters.PriceTo.HasValue)
            {
                products = products.Where(p => (p.NettoPrice / 100.0M + p.NettoPrice / 100.0M * (p.VATRate.Rate / 100.0M) <= filters.PriceTo));
            }
            if (filters.Attributes != null)
            {
                foreach (var value in filters.Attributes)
                {
                    switch (value.Type)
                    {
                        case AttributeType.Bool:
                        if (value.Value == "true")
                            products = products.Where(p => p.ProductAttributes.First(pa => pa.AttributeId == value.Id).Value == value.Value);
                        break;
                        case AttributeType.Text:
                        var query = value.Value.ToLower();
                        products = products.Where(p => p.ProductAttributes.First(pa => pa.AttributeId == value.Id).Value.ToLower().Contains(query) ||
                        query.Contains(p.ProductAttributes.First(pa => pa.AttributeId == value.Id).Value.ToLower()));
                        break;
                        case AttributeType.Numeric:
                        int val1, val2;
                        if (int.TryParse(value.ValueFrom, out val1))
                        {
                            products = products.Where(p => Convert.ToInt32(p.ProductAttributes.First(pa => pa.AttributeId == value.Id).Value) >= val1);
                        }
                        if (int.TryParse(value.ValueTo, out val2))
                        {
                            products = products.Where(p => Convert.ToInt32(p.ProductAttributes.First(pa => pa.AttributeId == value.Id).Value) <= val2);
                        }
                        break;
                        case AttributeType.Dictionary:
                        if (value.Values != null && value.Values.Length > 0)
                            products = products.Where(p => value.Values.Contains(p.ProductAttributes.First(pa => pa.AttributeId == value.Id).Value));
                        break;
                        case AttributeType.Date:
                        DateTime date1, date2;
                        if (DateTime.TryParse(value.ValueFrom, out date1))
                        {
                            products = products.Where(p => (DateTime)(object)(p.ProductAttributes.First(pa => pa.AttributeId == value.Id).Value) >= date1);
                        }
                        if (DateTime.TryParse(value.ValueTo, out date2))
                        {
                            products = products.Where(p => (DateTime)(object)(p.ProductAttributes.First(pa => pa.AttributeId == value.Id).Value) <= date2);
                        }
                        break;
                    }
                }
            }

            return products;
        }

        private IQueryable<Product> Sort(IQueryable<Product> products, ProductSortEnum sortOrder)
        {
            products = sortOrder switch
            {
                var sort when sort == ProductSortEnum.Default => products,
                var sort when sort == ProductSortEnum.NameASC => products.OrderBy(p => p.Name),
                var sort when sort == ProductSortEnum.NameDESC => products.OrderByDescending(p => p.Name),
                var sort when sort == ProductSortEnum.PriceASC => products.OrderBy(p => p.NettoPrice + p.NettoPrice * (p.VATRate.Rate / 100.0)),
                var sort when sort == ProductSortEnum.PriceDESC => products.OrderByDescending(p => p.NettoPrice + p.NettoPrice * (p.VATRate.Rate / 100.0)),
                _ => products
            };
            return products;
        }

        public async Task<List<Product>> GetProductsByCategoryId(int categoryId)
        {
            return await FindByCondition(p => p.ProductCategories.First(pc => pc.CategoryId == categoryId).CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<List<Product>> GetCardItems(List<int> list)
        {
            return await FindByCondition(p => list.Contains(p.Id))
                .Include(p => p.VATRate)
                .Include(p => p.Photos.Where(photo => photo.IsListPhoto))
                .ToListAsync();
        }

        public async Task<bool> CheckProductsActive(List<int> productsIds)
        {
            return !await FindByCondition(p => productsIds.Contains(p.Id) && p.Status != ProductStatus.Active).AnyAsync();
        }

        public async Task<List<Product>> GetProductsByIds(List<int> list)
        {
            return await FindByCondition(p => list.Contains(p.Id))
                .Include(p => p.VATRate)
                .ToListAsync();
        }
    }
}
