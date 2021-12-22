using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Shop.ViewModels.Profile;

namespace My_Company.Areas.Shop.ViewComponents
{
    public class ChangePersonalDataFormViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ChangePersonalDataModel model)
        {
            return View("ChangePersonalDataForm", model);
        }
    }
}
