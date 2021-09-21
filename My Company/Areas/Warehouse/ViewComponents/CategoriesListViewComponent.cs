using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Interfaces;
using My_Company.Models;
using My_Company.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewComponents
{
    public class CategoriesListViewComponent : ViewComponent
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public CategoriesListViewComponent(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(CategoryListFilters filters)
        {
            var categories = _repositoryWrapper.CategoriesRepository.GetCategoriesByFilters(filters); 

            var list = await PagedList<Category>
                .ToPagedList(categories, filters.Page.HasValue ? filters.Page.Value : 1, filters.PageSize.HasValue ? filters.PageSize.Value : 25);

            List<CategoryListItemViewModel> listView = new();

            foreach (var category in list)
            {
                var item = _mapper.Map<CategoryListItemViewModel>(category);
                item.CategoryTree = await _repositoryWrapper.CategoriesRepository.GetCategoryTree(category);
                item.Deletable = await _repositoryWrapper.CategoriesRepository.CheckIfRemovable(item.Id);
                listView.Add(item);
            }

            return View("CategoriesList", new PagedList<CategoryListItemViewModel>(listView, categories.Count(), list.CurrentPage, list.PageSize));
        }
    }
}
