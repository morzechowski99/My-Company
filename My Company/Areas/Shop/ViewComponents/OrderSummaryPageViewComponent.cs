using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Shop.ViewModels.Cart;
using My_Company.Areas.Shop.ViewModels.Order;
using My_Company.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static My_Company.Helpers.Constants;
using static My_Company.Helpers.CartHelpers;
using My_Company.EnumTypes;

namespace My_Company.Areas.Shop.ViewComponents
{
    public class OrderSummaryPageViewComponent : ViewComponent
    {
        private readonly IRepositoryWrapper repositoryWrapper;
        private readonly IMapper mapper;
        private readonly IConfig config;
        private readonly IParcelLockersService parcelLockersService;

        public OrderSummaryPageViewComponent(IRepositoryWrapper repositoryWrapper, IMapper mapper, IConfig config,
            IParcelLockersService parcelLockersService)
        {
            this.repositoryWrapper = repositoryWrapper;
            this.mapper = mapper;
            this.config = config;
            this.parcelLockersService = parcelLockersService;
        }

        public async Task<IViewComponentResult> InvokeAsync(NewOrderModel order)
        {
            OrderSummaryViewModel orderSummary = new();
            orderSummary.Order = order;
            var cartString = Request.Cookies[CART_COOKIE];
            var cart = JsonSerializer.Deserialize<List<CartCookieItem>>(cartString);
            
            var productsInCart = await repositoryWrapper.ProductRepository.GetCardItems(cart.Select(i => i.Id).ToList());
            var cartItems = mapper.Map<List<CartItem>>(productsInCart);
            cartItems.ForEach(ci =>
            {
                ci.Quantity = cart.FirstOrDefault(c => c.Id == ci.Id).Quantity;
                ci.Price = ci.Quantity * ci.Price;
            });
            Cart cartView = new Cart { Items = cartItems, Total = GetCartTotal(cartItems) };
            orderSummary.Cart = cartView;
            orderSummary.ShippingValue = (await config.GetShippingPrice(order.DeliveryType, repositoryWrapper.ConfigRepository)) / 100.0M;
            orderSummary.PaymentValue = (await config.GetPaymentPrice(order.PaymentMethod, repositoryWrapper.ConfigRepository)) / 100.0M;
            orderSummary.Total = cartView.Total + orderSummary.ShippingValue + orderSummary.PaymentValue;
            if(order.DeliveryType == DeliveryType.PaczkomatyInPost)
            {
                orderSummary.ParcelLocker = await parcelLockersService.GetParcelLockerInfo(order.PackLockerName);
            }

            return View("OrderSummaryPage", orderSummary);
        }
    }
}
