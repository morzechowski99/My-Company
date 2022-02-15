//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Warehouse.ViewModels;

namespace My_Company.Areas.Warehouse.ViewComponents
{
    public class EditPhotoModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int order)
        {
            return View("EditPhotoModal", new EditMainPagePhotoViewModel { Order = order });
        }
    }
}
