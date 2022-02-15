//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Warehouse.ViewModels;

namespace My_Company.Areas.Warehouse.ViewComponents
{
    public class AddRowModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("AddRowModal", new NewWarehouseSectorViewModel());
        }
    }
}
