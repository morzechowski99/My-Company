//Program powstał na Wydziale Informatyki Politechniki Białostockiej
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
    public class DeliveriesListViewComponent : ViewComponent
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public DeliveriesListViewComponent(IMapper mapper, IRepositoryWrapper repositoryWrapper)
        {
            this._mapper = mapper;
            this._repositoryWrapper = repositoryWrapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(DeliveriesListFilters filters)
        {
            var deliveries = _repositoryWrapper.DeliveriesRepository.GetDeliveriesByFilters(filters);

            var list = await PagedList<Delivery>
                .ToPagedList(deliveries, filters.Page.HasValue ? filters.Page.Value : 1, filters.PageSize.HasValue ? filters.PageSize.Value : 25);

            var listView = _mapper.Map<List<DeliveryListItem>>(list);

            return View("DeliveriesList", new PagedList<DeliveryListItem>(listView, deliveries.Count(), list.CurrentPage, list.PageSize));
        }
    }
}
