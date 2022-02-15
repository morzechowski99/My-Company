//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewComponents
{
    public class PickingDetailsViewComponent : ViewComponent
    {
        private readonly IMapper mapper;
        private readonly IRepositoryWrapper repositoryWrapper;

        public PickingDetailsViewComponent(IMapper mapper, IRepositoryWrapper repositoryWrapper)
        {
            this.mapper = mapper;
            this.repositoryWrapper = repositoryWrapper;
        }
        public async Task<IViewComponentResult> InvokeAsync(IEnumerable<PickedItemViewModel> pikingItems, Guid? pickingId)
        {
            if (pickingId != null)
            {
                var pickingItems = await repositoryWrapper.PickingRepository.GetItems(pickingId.Value);
                var orderPikingItemsDtos = mapper.Map<List<PickedItemViewModel>>(pickingItems);
                return View("PickingDetails", orderPikingItemsDtos);
            }
            else
            {
                return View("PickingDetails", pikingItems);
            }
        }
    }
}
