using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Warehouse.ViewModels.Account;
using My_Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static My_Company.Helpers.Constants;

namespace My_Company.Areas.Warehouse.Controllers
{
    [Area("Warehouse")]
    [Authorize(Policy = AuthorizationPolicies.WarehousePolicy)]
    public class HomeController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper mapper;

        public HomeController(SignInManager<AppUser> signInManager, IMapper mapper)
        {
            _signInManager = signInManager;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            if (User.IsInRole(Roles.WarehouseEmployee))
                return View("Index",User.Identity.Name);
            if (User.IsInRole(Roles.MainAdministrator))
                return View("AdminDashboard");
            else
                return Unauthorized();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            await _signInManager.SignOutAsync();
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager
                    .PasswordSignInAsync(loginModel.Username, loginModel.Password, loginModel.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                if (result.IsLockedOut)
                {
                    return LocalRedirect("/Identity/Account/Lockout");
                }
                else
                {
                    ViewBag.LoginMessage = "Podana nazwa użytkownika i/lub hasło są nieprawidłowe";
                    return View(loginModel);
                }
            }
            return View(loginModel);
        }
    }
}
