using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Shop.ViewModels.Profile;
using My_Company.Extensions;
using My_Company.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Shop.ViewComponents
{
    public class UserOrdersViewComponent : ViewComponent
    {
        private readonly IRepositoryWrapper repositoryWrapper;
        private readonly IMapper mapper;
        private readonly HttpContext context;

        public UserOrdersViewComponent(IRepositoryWrapper repositoryWrapper, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.repositoryWrapper = repositoryWrapper;
            this.mapper = mapper;
            context = httpContextAccessor.HttpContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var orders = await repositoryWrapper.OrdersRepository.GetOrdersByUserId(context.User.GetId());
            var ordersDtos = mapper.Map<List<UserOrderListItemViewModel>>(orders);
            return View("UserOrders", ordersDtos);
        }
    }
}
