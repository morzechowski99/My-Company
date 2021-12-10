using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using My_Company.Models;
using System.Threading.Tasks;

namespace My_Company.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {

        private readonly SignInManager<AppUser> _signInManager;

        public LoginModel(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGet(string returnUrl = null)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            if (returnUrl != null && returnUrl.ToLower().StartsWith("/warehouse"))
            {
                return RedirectToAction("Login", "Home", new { area = "Warehouse" });
            }

            return RedirectToAction("Login", "MyAccount", new { area = "Shop" });
        }
    }
}
