using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Interfaces;
using My_Company.Models;
using My_Company.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewComponents
{
    public class OrdersListViewComponent : ViewComponent
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public OrdersListViewComponent(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(OrdersListFilters filters)
        {
            var orders = _repositoryWrapper.OrdersRepository.GetOrdersByFilters(filters);

            var list = await PagedList<Order>
                .ToPagedList(orders, filters.Page.HasValue ? filters.Page.Value : 1, filters.PageSize.HasValue ? filters.PageSize.Value : 25);

            var listView = _mapper.Map<List<AllOrdersListItemViewModel>>(list);

            return View("OrdersList", new PagedList<AllOrdersListItemViewModel>(listView, orders.Count(), list.CurrentPage, list.PageSize));
        }
    }
}
