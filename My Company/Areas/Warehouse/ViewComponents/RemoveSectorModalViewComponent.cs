//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using Microsoft.AspNetCore.Mvc;

namespace My_Company.Areas.Warehouse.ViewComponents
{
    public class RemoveSectorModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("RemoveSectorModal");
        }
    }
}
