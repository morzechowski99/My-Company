using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Helpers;
using My_Company.Interfaces;
using My_Company.Models;
using My_Company.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewComponents
{
    public class ProductsListViewComponent : ViewComponent
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public ProductsListViewComponent(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(ProductsListFilters filters)
        {
            var products = _repositoryWrapper.ProductRepository.GetProductsByFilters(filters);

            var list = await PagedList<Product>
                .ToPagedList(products, filters.Page.HasValue ? filters.Page.Value : 1, filters.PageSize.HasValue ? filters.PageSize.Value : 25);

            List<ProductListItemViewModel> listView = new();

            foreach (var product in list)
            {
                var item = _mapper.Map<ProductListItemViewModel>(product);
                Photo photo = product.Photos.FirstOrDefault(p => p.IsListPhoto);
                item.PhotoUrl = photo == null ? Constants.ImagePlaceholder : photo.Path;
                item.StockStatus = ViewHelpers.GetProductStockStatus(product);
                listView.Add(item);
            }

            return View("ProductsList", new PagedList<ProductListItemViewModel>(listView, products.Count(), list.CurrentPage, list.PageSize));
        }
    }
}
