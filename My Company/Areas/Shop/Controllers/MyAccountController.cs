using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using My_Company.Areas.Shop.ViewModels.Login;
using My_Company.Areas.Shop.ViewModels.Profile;
using My_Company.Extensions;
using My_Company.Helpers;
using My_Company.Interfaces;
using My_Company.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace My_Company.Areas.Shop.Controllers
{
    [Area("Shop")]
    [Authorize(Policy = Constants.AuthorizationPolicies.ShopAccountPolicy)]
    public class MyAccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUsersService usersService;
        private readonly IMapper mapper;
        private readonly IEmailService emailService;
        private readonly IRepositoryWrapper repositoryWrapper;
        private readonly IOrdersService ordersService;

        public MyAccountController(SignInManager<AppUser> signInManager, IUsersService usersService, IMapper mapper, IEmailService emailService,
            IRepositoryWrapper repositoryWrapper, IOrdersService ordersService)
        {
            _signInManager = signInManager;
            this.usersService = usersService;
            this.mapper = mapper;
            this.emailService = emailService;
            this.repositoryWrapper = repositoryWrapper;
            this.ordersService = ordersService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await usersService.GetUserById(HttpContext.User.GetId());

            return View(mapper.Map<UserViewModel>(user));
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager
                    .PasswordSignInAsync(loginModel.Email, loginModel.Password, loginModel.RememberMe, lockoutOnFailure: false);
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
                    ViewBag.LoginMessage = "Podany email lub hasło są nieprawidłowe";
                    return View(mapper.Map<LoginRegisterModel>(loginModel));
                }
            }
            return View(mapper.Map<LoginRegisterModel>(loginModel));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var result = await usersService.CreateShopUser(registerModel);
                if (result.Succeeded)
                {
                    var verifyData = await usersService.GenerateEmailConfirmationData(registerModel.EmailRegister);
                    var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(verifyData.VerifyCode));
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "MyAccount",
                        values: new { area = "Shop", userId = verifyData.UserId, code = code },
                        protocol: Request.Scheme);

                    emailService.SendRegistrationEmail(registerModel.EmailRegister, callbackUrl);

                    TempData["message"] = "Twoje konto zostało założone. Aby zalogować się do sklepu potwierdź je" +
                        "klikajać w link w wiadomości email";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.RegisterMessage = new List<string>();
                    foreach (var error in result.Errors)
                    {
                        (ViewBag.RegisterMessage as List<string>).Add(error.Description);
                    }
                    return View("Login", mapper.Map<LoginRegisterModel>(registerModel));
                }

            }
            return View("Login", mapper.Map<LoginRegisterModel>(registerModel));
        }

        public async Task<IActionResult> Logout(string returnUrl)
        {
            await _signInManager.SignOutAsync();
            if (returnUrl != null)
                return Redirect(returnUrl);

            else return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var result = await usersService.ConfirmEmail(userId, code);

            if (result)
            {
                TempData["message"] = "Email potwierdzony pomyślnie. Możesz się zalogować";
                return RedirectToAction(nameof(Login));
            }
            else
            {
                TempData["message"] = "Podczas potwierdzenia maila wystąpił błąd";
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
                return ViewComponent("ChangePasswordForm", model);

            var user = await usersService.GetUserById(User.GetId());
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{User.GetId()}'.");
            }

            var changePasswordResult = await usersService.ChangePassword(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return ViewComponent("ChangePasswordForm", model);
            }

            await _signInManager.RefreshSignInAsync(user);
            TempData["message"] = "Hasło zmienione pomyślnie";
            return ViewComponent("ChangePasswordForm", new ChangePasswordModel());
        }

        [HttpPost]
        public async Task<IActionResult> ChangePersonalData(ChangePersonalDataModel model)
        {
            if (!ModelState.IsValid)
                return ViewComponent("ChangePersonalDataForm", model);

            var user = await usersService.GetUserById(User.GetId());
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{User.GetId()}'.");
            }

            user.Name = model.Name;
            user.Surname = model.Surname;

            await usersService.UpdateUser(user);

            TempData["message"] = "Zapisano pomyślnie";
            return ViewComponent("ChangePersonalDataForm", model);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAddress(int? id)
        {
            if (id == null)
                return BadRequest();

            var address = await repositoryWrapper.AddressesRepository.GetOne(a => a.Id == id && a.UserId == User.GetId());
            if (address == null)
                return NotFound();

            address.UserId = null;
            repositoryWrapper.AddressesRepository.Update(address);
            await repositoryWrapper.Save();
            return Ok();
        }

        public async Task<IActionResult> OrderDetails(Guid? id)
        {
            if (id == null)
                return BadRequest();

            var orderModel = await ordersService.GetOrderByIdAndUser(id.Value,User.GetId());

            return View(orderModel);
        }

    }
}
