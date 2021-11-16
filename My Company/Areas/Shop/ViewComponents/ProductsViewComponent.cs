using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Shop.ViewModels.Products;
using My_Company.Helpers;
using My_Company.Interfaces;
using My_Company.Models;
using My_Company.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Shop.ViewComponents
{
    public class ProductsViewComponent : ViewComponent
    {
        private readonly IMapper mapper;
        private readonly IRepositoryWrapper repositoryWrapper;

        public ProductsViewComponent(IMapper mapper, IRepositoryWrapper repositoryWrapper)
        {
            this.mapper = mapper;
            this.repositoryWrapper = repositoryWrapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(ProductsListFilters filters)
        {
            var categories = await repositoryWrapper.CategoriesRepository.GetAllCategoriesInTree(filters.CategoryId);
            var products = repositoryWrapper.ProductRepository.GetByFilters(filters);

            var list = await PagedList<Product>
                .ToPagedList(products, filters.Page.HasValue ? filters.Page.Value : 1, filters.PageSize.HasValue ? filters.PageSize.Value : 12);

            List<ListItemViewModel> listView = new();

            foreach (var product in list)
            {
                var item = mapper.Map<ListItemViewModel>(product);
                Photo photo = product.Photos.FirstOrDefault(p => p.IsListPhoto);
                item.PhotoUrl = photo == null ? Constants.ImagePlaceholder : photo.Path;
                listView.Add(item);
            }

            return View("Products", new PagedList<ListItemViewModel>(listView, list.TotalCount, list.CurrentPage, list.PageSize));
        }
    }
}
