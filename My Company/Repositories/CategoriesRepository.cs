using Microsoft.EntityFrameworkCore;
using My_Company.Areas.Warehouse.EnumTypes;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Data;
using My_Company.Interfaces;
using My_Company.Models;
using My_Company.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Repositories
{
    public class CategoriesRepository : RepositoryBase<Category>, ICategoriesRepository
    {
        public CategoriesRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> CheckName(string categoryName)
        {
            return await FindAll().AnyAsync(c => c.CategoryName == categoryName);
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await FindAll().ToListAsync();
        }

        public async Task<IEnumerable<Models.Attribute>> GetAllAttributesByCategoryId(int? categoryId)
        {
            var categories = await FindAll().Include(c => c.Attributes).ToListAsync();
            var category = categories.FirstOrDefault(c => c.Id == categoryId);
            if (category == null)
                return null;
            List<Models.Attribute> attributes  = new();
            while(category != null)
            {
                if(category.Attributes != null)
                    attributes.AddRange(category.Attributes);

                category = categories.FirstOrDefault(c => c.Id == category.ParentCategoryId);
            }

            return attributes;
        }  
        
        public async Task<IEnumerable<Models.Attribute>> GetAllAttributesByCategoryIdWithDictionaryValues(int? categoryId)
        {
            var categories = await FindAll()
                .Include(c => c.Attributes)
                .ThenInclude(a => a.AttributeDictionaryValues)
                .ToListAsync();

            var category = categories.FirstOrDefault(c => c.Id == categoryId);
            if (category == null)
                return null;
            List<Models.Attribute> attributes  = new();
            while(category != null)
            {
                if(category.Attributes != null)
                    attributes.AddRange(category.Attributes);

                category = categories.FirstOrDefault(c => c.Id == category.ParentCategoryId);
            }

            return attributes;
        }

        public async Task<IEnumerable<CategoryTree>> GetCategoriesTree()
        {
            var categories = await GetAll();
            List<CategoryTree> categoryTrees = new();

            foreach (var category in categories)
            {
                string tree = category.CategoryName;
                int? parent = category.ParentCategoryId;
                if (parent.HasValue)
                {
                    while (parent.HasValue)
                    {
                        var parentCategory = categories.FirstOrDefault(c => c.Id == parent.Value);
                        tree = parentCategory.CategoryName + "/\n" + tree;
                        parent = parentCategory.ParentCategoryId;       
                    }
                }
                categoryTrees.Add(new()
                {
                    Id = category.Id,
                    Tree = tree
                }
                );
            }
            return categoryTrees.OrderBy(c => c.Tree);
        }

        public async Task<string> GetCategoryTree(Category category)
        {
            string tree = "";
            int? parent = category.ParentCategoryId;
            if (parent.HasValue)
            {
                while (parent.HasValue)
                {
                    var parentCategory = await GetById(parent.Value);
                    tree = parentCategory.CategoryName + "/" + tree;
                    parent = parentCategory.ParentCategoryId;
                }
            }
            else return "-";
            return tree;
        }
        
        public async Task<string> GetCategoryTreeWithCategoryName(Category category)
        {
            string tree = "";
            int? parent = category.ParentCategoryId;
            if (parent.HasValue)
            {
                while (parent.HasValue)
                {
                    var parentCategory = await GetById(parent.Value);
                    tree = parentCategory.CategoryName + "/" + tree;
                    parent = parentCategory.ParentCategoryId;
                }
            }
            else return category.CategoryName;
            return tree + $"{category.CategoryName}";
        }

        public async Task<Category> GetById(int id)
        {
            return await FindByCondition(c => c.Id == id).FirstOrDefaultAsync();
        }

        public IQueryable<Category> GetCategoriesByFilters(CategoryListFilters filters)
        {
            var query = FindAll();

            if (!string.IsNullOrEmpty(filters.SearchString))
                query = query.Where(c =>
                c.CategoryName.Contains(filters.SearchString) ||
                filters.SearchString.Contains(c.CategoryName) ||
                c.Description.Contains(filters.SearchString) ||
                filters.SearchString.Contains(c.Description));

            switch (filters.SortOrder)
            {
                case CategoriesSortOrderEnum.NameASC:
                    query = query.OrderBy(c => c.CategoryName);
                    break;
                case CategoriesSortOrderEnum.NameDESC:
                    query = query.OrderByDescending(c => c.CategoryName);
                    break;
            }

            return query;
        }

        public async Task<bool> CheckIfRemovable(int id)
        {
            return !await FindByCondition(c => c.Id == id).AnyAsync(c => c.ChildCategories.Any() || c.ProductCategories.Any());
        }

        public async Task<Category> GetCategoryWithAttributes(int id)
        {
            return await FindByCondition(item => item.Id == id)
                .Include(c => c.Attributes)
                .ThenInclude(a => a.AttributeDictionaryValues)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CheckNameToEdit(string categoryName, int categoryId)
        {
            return await FindAll().AnyAsync(c => c.CategoryName == categoryName && c.Id != categoryId);
        }

        public async Task<Category> GetCategoryWithAttributesTracked(int id)
        {
            return await GetTracked().Include(c => c.Attributes).FirstOrDefaultAsync(c => c.Id == id);
        }

        public IQueryable<Category> ChildCategoriesById(int? id)
        {
            if (id == null)
                return FindByCondition(c => c.ParentCategoryId == null);
            else
                return FindByCondition(c => c.ParentCategoryId == id);
        }
    }
}
