using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Shop.ViewModels.Cart;
using My_Company.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static My_Company.Helpers.CartHelpers;
using static My_Company.Helpers.Constants;

namespace My_Company.Areas.Shop.ViewComponents
{
    public class OrderSummaryViewComponent : ViewComponent
    {
        private readonly IRepositoryWrapper repositoryWrapper;
        private readonly IMapper mapper;

        public OrderSummaryViewComponent(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            this.repositoryWrapper = repositoryWrapper;
            this.mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<CartCookieItem> cart = null;
            var cartString = Request.Cookies[CART_COOKIE];
            if (cartString != null)
                cart = JsonSerializer.Deserialize<List<CartCookieItem>>(cartString);
            else
                cart = new();

            var productsInCart = await repositoryWrapper.ProductRepository.GetCardItems(cart.Select(i => i.Id).ToList());
            var cartItems = mapper.Map<List<CartItem>>(productsInCart);
            cartItems.ForEach(ci =>
                {
                    ci.Quantity = cart.FirstOrDefault(c => c.Id == ci.Id).Quantity;
                    ci.Price = ci.Quantity * ci.Price;
                });
            var total = GetCartTotal(cartItems);
            return View("OrderSummary", total);
        }
    }
}
