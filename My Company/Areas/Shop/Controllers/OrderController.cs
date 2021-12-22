using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Shop.ViewModels.Cart;
using My_Company.Areas.Shop.ViewModels.Login;
using My_Company.Areas.Shop.ViewModels.Order;
using My_Company.EnumTypes;
using My_Company.Extensions;
using My_Company.Filters;
using My_Company.Helpers;
using My_Company.Interfaces;
using My_Company.Models;
using My_Company.Services.PaymentService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using static My_Company.Helpers.Constants;

namespace My_Company.Areas.Shop.Controllers
{
    [Area("Shop")]
    public class OrderController : Controller
    {
        private readonly IRepositoryWrapper repositoryWrapper;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper mapper;
        private readonly IOrdersService ordersService;
        private readonly IServiceProvider serviceProvider;
        private readonly IConfig config;
        private readonly IEmailService emailService;
        public OrderController(IRepositoryWrapper repositoryWrapper, SignInManager<AppUser> signInManager,
            IMapper mapper, IOrdersService ordersService, IServiceProvider serviceProvider, IConfig config,
            IEmailService emailService)
        {
            this.repositoryWrapper = repositoryWrapper;
            _signInManager = signInManager;
            this.mapper = mapper;
            this.ordersService = ordersService;
            this.serviceProvider = serviceProvider;
            this.config = config;
            this.emailService = emailService;
        }

        public async Task<IActionResult> New()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Login));
            }
            List<CartCookieItem> cart = null;
            var cartString = Request.Cookies[CART_COOKIE];
            if (cartString == null || (cart = JsonSerializer.Deserialize<List<CartCookieItem>>(cartString)).Count == 0)
            {
                return RedirectToAction("Cart", "Cart");
            }
            if (!await repositoryWrapper.ProductRepository.CheckProductsActive(cart.Select(ci => ci.Id).ToList()))
            {
                TempData["productNotActive"] = "Jeden z produktów z twojego koszyka jest niedostępny. Twój koszyk został wyczyszczony. Przepraszamy";
                Response.Cookies.Delete(CART_COOKIE);
                return RedirectToAction("Cart", "Cart");
            }
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager
                    .PasswordSignInAsync(loginModel.Email, loginModel.Password, loginModel.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(New));
                }
                if (result.IsLockedOut)
                {
                    return LocalRedirect("/Identity/Account/Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Podany email lub hasło są nieprawidłowe");
                    return View(loginModel);
                }
            }
            return View(loginModel);
        }

        public async Task<IActionResult> NewFromGuest()
        {
            List<CartCookieItem> cart = null;
            var cartString = Request.Cookies[CART_COOKIE];
            if (cartString == null || (cart = JsonSerializer.Deserialize<List<CartCookieItem>>(cartString)).Count == 0)
            {
                return RedirectToAction("Cart", "Cart");
            }
            if (!await repositoryWrapper.ProductRepository.CheckProductsActive(cart.Select(ci => ci.Id).ToList()))
            {
                TempData["productNotActive"] = "Jeden z produktów z twojego koszyka jest niedostępny. Twój koszyk został wyczyszczony. Przepraszamy";
                Response.Cookies.Delete(CART_COOKIE);
                return RedirectToAction("Cart", "Cart");
            }

            return View("New");
        }

        [HttpPost]
        public IActionResult GetSummary(NewOrderModel orderModel)
        {
            if (orderModel == null)
                return BadRequest();

            return ViewComponent("OrderSummaryPage", orderModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(NewOrderModel orderModel)
        {
            if (orderModel == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return View(orderModel);
            if (orderModel.DeliveryType == DeliveryType.PaczkomatyInPost && orderModel.PackLockerName == null)
            {
                ModelState.AddModelError("PackLockerName", "Wybierz paczkomat");
            }

            List<CartCookieItem> cart = null;
            var cartString = Request.Cookies[CART_COOKIE];
            if (cartString == null || (cart = JsonSerializer.Deserialize<List<CartCookieItem>>(cartString)).Count == 0)
            {
                return RedirectToAction("Cart", "Cart");
            }
            if (!await repositoryWrapper.ProductRepository.CheckProductsActive(cart.Select(ci => ci.Id).ToList()))
            {
                TempData["productNotActive"] = "Jeden z produktów z twojego koszyka jest niedostępny. Twój koszyk został wyczyszczony. Przepraszamy";
                Response.Cookies.Delete(CART_COOKIE);
                return RedirectToAction("Cart", "Cart");
            }

            string userId = null;
            if (User.Identity.IsAuthenticated)
                userId = User.GetId();

            using var tr = await repositoryWrapper.BeginTransaction();
            var order = await ordersService.CreateOrder(orderModel, cart, userId);
            if (order == null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            var paymentService = order.PaymentMethod.GetService(serviceProvider);
            var callback = await paymentService.GetLinkToPayment(order);

            Response.Cookies.Delete(CART_COOKIE);
            await tr.CommitAsync();
            string url = GetOrderDetailsUrl(order);
            emailService.SendOrderEmail(OrderEmailReason.NewOrder, order, url, User.Identity.IsAuthenticated ? User.GetEmail() : null);

            if (callback != null)
            {
                return Redirect(callback);
            }
            else
            {
                return RedirectToAction(nameof(PaymentConfirm), new { orderId = order.Id });
            }
        }

        private string GetOrderDetailsUrl(Order order)
        {
            return Url.Action("OrderDetails", "MyAccount", values: new { area = "Shop", id = order.Id }, protocol: Request.Scheme);
        }

        public async Task<IActionResult> PaymentConfirm(Guid? orderId, Guid? control)
        {
            if (orderId == null && control == null)
                return BadRequest();

            var id = orderId == null ? control.Value : orderId.Value;
            var orderType = await repositoryWrapper.OrdersRepository.GetOrderPaymentTypeByOrderId(id);

            if (orderType == null)
                return NotFound();

            ViewBag.OrderNr = id;

            return View(orderType);
        }

        [HttpPost]
        [ServiceFilter(typeof(DotPayIpFilter))]
        public async Task<IActionResult> PaymentStatus(DotPayURLCResponse dotpayResponse)
        {
            if (dotpayResponse == null)
                return BadRequest();

            var id = await config.GetValue(Constants.ConfigKeys.DotPayKeys.Id, repositoryWrapper.ConfigRepository);
            if (dotpayResponse.Id != int.Parse(id))
                return BadRequest("invalid shop Id");

            if (dotpayResponse.Operation_currency != dotpayResponse.Operation_original_currency
               && dotpayResponse.Operation_currency != "PLN")
            {
                return BadRequest("invalid currency");
            }

            var order = await ordersService.GetOrderWithPaymentAndUserById(dotpayResponse.Control);
            if (order == null)
                return NotFound();

            var orderTotal = OrderHelpers.GetOrderAmmount(order);
            if (orderTotal != decimal.Parse(dotpayResponse.Operation_amount.Replace('.', ',')))
                return BadRequest("invalid amount");

            if (dotpayResponse.Operation_status == "completed")
            {
                order.Status = OrderStatus.Paid;
                order.Payment.Status = EnumTypes.PaymentStatus.Completed;
                order.Paid = true;
                emailService.SendOrderEmail(OrderEmailReason.ChangeOrderStatus, order, GetOrderDetailsUrl(order), order.User?.Email);
            }
            else if (dotpayResponse.Operation_status == "rejected")
            {
                order.Payment.Status = EnumTypes.PaymentStatus.Rejected;
            }

            order.ProductOrders = null;

            repositoryWrapper.OrdersRepository.Update(order);
            await repositoryWrapper.Save();

            return Ok();
        }
    }
}
