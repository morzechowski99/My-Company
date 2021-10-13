﻿using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Models;
using My_Company.ViewModels;
using System;
using System.Collections;
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
        IQueryable<Category> GetCategoriesByFilters(CategoryListFilters filters);
        Task<string> GetCategoryTree(Category category);
        Task<bool> CheckIfRemovable(int id);
        Task<Category> GetById(int id);
        Task<Category> GetCategoryWithAttributes(int id);
        Task<bool> CheckNameToEdit(string categoryName,int categoryId);
        Task<Category> GetCategoryWithAttributesTracked(int id);
        IQueryable<Category> ChildCategoriesById(int? id);
        Task<IEnumerable<Models.Attribute>> GetAllAttributesByCategoryIdWithDictionaryValues(int? categoryId);
    }
}
