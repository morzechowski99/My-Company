using Microsoft.EntityFrameworkCore;
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
                        tree = parentCategory.CategoryName + "/" + tree;
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
            return categoryTrees;
        }
    }
}
