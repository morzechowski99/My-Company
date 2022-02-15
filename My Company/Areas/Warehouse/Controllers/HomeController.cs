//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Areas.Warehouse.ViewModels.Account;
using My_Company.Interfaces;
using My_Company.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static My_Company.Areas.Warehouse.EnumTypes.ChartEnums;
using static My_Company.Helpers.Constants;

namespace My_Company.Areas.Warehouse.Controllers
{
    [Area("Warehouse")]
    [Authorize(Policy = AuthorizationPolicies.WarehousePolicy)]
    public class HomeController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper mapper;
        private readonly IRepositoryWrapper repositoryWrapper;

        public HomeController(SignInManager<AppUser> signInManager, IMapper mapper, IRepositoryWrapper repositoryWrapper)
        {
            _signInManager = signInManager;
            this.mapper = mapper;
            this.repositoryWrapper = repositoryWrapper;
        }

        public IActionResult Index()
        {
            if (User.IsInRole(Roles.WarehouseEmployee))
                return View("Index", User.Identity.Name);
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

        [HttpGet]
        public async Task<IActionResult> GetChartData(ChartMode mode, ChartRange range)
        {
            List<ChartItem> items = mode switch
            {
                var m when m == ChartMode.Orders => await repositoryWrapper.OrdersRepository.GetDataToChart(range),
                var m when m == ChartMode.Completions => await repositoryWrapper.PickingRepository.GetDataToChart(range),
                var m when m == ChartMode.Packing => await repositoryWrapper.OrderPackingRepository.GetDataToChart(range),
                _ => null,
            };
            return Ok(items);
        }
    }
}
