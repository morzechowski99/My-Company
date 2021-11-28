using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Shop.ViewModels.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
