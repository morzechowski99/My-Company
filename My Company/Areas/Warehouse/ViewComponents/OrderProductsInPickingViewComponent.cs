using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Helpers;
using My_Company.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewComponents
{
    public class OrderProductsInPickingViewComponent : ViewComponent
    {
        private readonly IMapper mapper;
        private readonly IRepositoryWrapper repositoryWrapper;

        public OrderProductsInPickingViewComponent(IMapper mapper, IRepositoryWrapper repositoryWrapper)
        {
            this.mapper = mapper;
            this.repositoryWrapper = repositoryWrapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(List<OrderPickingItemViewModel> orderPikingItems, Guid? orderId)
        {
            if (orderId != null)
            {
                var order = await repositoryWrapper.OrdersRepository.GetOrderWithProductsInfoById(orderId.Value);
                if (order == null)
                    return null;
                var orderPikingItemsDtos = mapper.Map<List<OrderPickingItemViewModel>>(order.ProductOrders);
                foreach (var item in orderPikingItemsDtos)
                {
                    item.Completed = OrderHelpers.GetCompletedCount(order.Picking.PickingItems, item.ProductOrderId);
                    var product = order.ProductOrders
                        .FirstOrDefault(po => po.Id == item.ProductOrderId)
                        .Product;
                    var photo = product.Photos.FirstOrDefault(p => p.IsListPhoto);
                    var photoUrl = photo == null ? Constants.ImagePlaceholder : photo.Path;
                    var productDto = mapper.Map<ProductListItemViewModel>(product);
                    productDto.PhotoUrl = photoUrl;
                    item.Product = productDto;
                }
                return View("OrderProductsInPicking", orderPikingItemsDtos);
            }
            else
            {
                return View("OrderProductsInPicking", orderPikingItems);
            }
        }
    }
}
