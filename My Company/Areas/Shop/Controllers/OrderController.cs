﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Shop.ViewModels.Cart;
using My_Company.Areas.Shop.ViewModels.Login;
using My_Company.Areas.Shop.ViewModels.Order;
using My_Company.EnumTypes;
using My_Company.Extensions;
using My_Company.Interfaces;
using My_Company.Models;
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

        public OrderController(IRepositoryWrapper repositoryWrapper, SignInManager<AppUser> signInManager, IMapper mapper, IOrdersService ordersService)
        {
            this.repositoryWrapper = repositoryWrapper;
            _signInManager = signInManager;
            this.mapper = mapper;
            this.ordersService = ordersService;
        }

        public async Task<IActionResult> New()
        {
            if(!User.Identity.IsAuthenticated)
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
            if(!await repositoryWrapper.ProductRepository.CheckProductsActive(cart.Select(ci => ci.Id).ToList()))
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
            if (orderModel == null )
                return BadRequest();
            if (!ModelState.IsValid)
                return View(orderModel);
            if(orderModel.DeliveryType == DeliveryType.PaczkomatyInPost && orderModel.PackLockerName == null)
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

            if (!await ordersService.CreateOrder(orderModel,cart,userId))
                return StatusCode(StatusCodes.Status500InternalServerError);

            Response.Cookies.Delete(CART_COOKIE);

            return View();
        }
    }
}
