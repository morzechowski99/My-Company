using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Shop.ViewModels.Profile;

namespace My_Company.Areas.Shop.ViewComponents
{
    public class ChangePasswordFormViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ChangePasswordModel model)
        {
            return View("ChangePasswordForm", model);
        }
    }
}
