//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Shop.ViewModels.Cart;
using My_Company.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static My_Company.Helpers.CartHelpers;
using static My_Company.Helpers.Constants;

namespace My_Company.Areas.Shop.Controllers
{
    [Area("Shop")]
    public class CartController : Controller
    {
        private readonly IRepositoryWrapper repositoryWrapper;
        private readonly IMapper mapper;
        private readonly CookieOptions cookieOptions;

        public CartController(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            this.repositoryWrapper = repositoryWrapper;
            this.mapper = mapper;
            cookieOptions = new CookieOptions { Expires = DateTime.Now.AddYears(1) };
        }

        [HttpPost]
        public IActionResult AddToCart(AddToCartModel cartModel)
        {
            if (cartModel == null || !ModelState.IsValid)
                return BadRequest();
            List<CartCookieItem> cart = null;
            var cartString = Request.Cookies[CART_COOKIE];
            if (cartString != null)
                cart = JsonSerializer.Deserialize<List<CartCookieItem>>(cartString);
            else
                cart = new();

            var item = cart.FirstOrDefault(i => i.Id == cartModel.ProductId);
            if (item != null)
                item.Quantity += cartModel.Count;
            else
                cart.Add(new CartCookieItem { Id = cartModel.ProductId, Quantity = cartModel.Count });

            Response.Cookies.Append(CART_COOKIE, JsonSerializer.Serialize(cart), cookieOptions);
            return ViewComponent("Cart", new { cart = cart });
        }

        [HttpDelete]
        public IActionResult RemoveItem(int? id)
        {
            if (id == null)
                return BadRequest();
            List<CartCookieItem> cart = null;
            var cartString = Request.Cookies[CART_COOKIE];
            if (cartString == null)
                return BadRequest();
            cart = JsonSerializer.Deserialize<List<CartCookieItem>>(cartString);
            cart = cart.Where(ci => ci.Id != id.Value).ToList();

            Response.Cookies.Append(CART_COOKIE, JsonSerializer.Serialize(cart), cookieOptions);
            return ViewComponent("Cart", new { cart = cart });
        }

        [HttpGet]
        public async Task<IActionResult> Cart()
        {
            List<CartCookieItem> cartItems = null;
            var cartString = Request.Cookies[CART_COOKIE];
            if (cartString != null)
                cartItems = JsonSerializer.Deserialize<List<CartCookieItem>>(cartString);
            else
                cartItems = new();

            var productsInCart = await repositoryWrapper.ProductRepository.GetCardItems(cartItems.Select(i => i.Id).ToList());
            var cart = mapper.Map<List<CartItem>>(productsInCart);
            cart.ForEach(ci =>
            {
                ci.Quantity = cartItems.FirstOrDefault(c => c.Id == ci.Id).Quantity;
                ci.OneItemPrice = ci.Price;
                ci.Price = ci.Quantity * ci.Price;
            });
            Cart cartView = new Cart { Items = cart, Total = GetCartTotal(cart) };


            return View(cartView);
        }
    }
}
