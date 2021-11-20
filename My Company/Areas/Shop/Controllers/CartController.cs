using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Shop.ViewModels.Cart;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using static My_Company.Helpers.Constants;

namespace My_Company.Areas.Shop.Controllers
{
    [Area("Shop")]
    public class CartController : Controller
    {
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

            Response.Cookies.Append(CART_COOKIE, JsonSerializer.Serialize(cart));
            return ViewComponent("Cart",new { cart = cart});
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

            Response.Cookies.Append(CART_COOKIE, JsonSerializer.Serialize(cart));
            return ViewComponent("Cart",new { cart = cart});
        }
    }
}
