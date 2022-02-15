//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Warehouse.ViewModels;

namespace My_Company.Areas.Warehouse.ViewComponents
{
    public class ChangeStatusModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(OrderDetailsViewModel order)
        {
            return View("ChangeStatusModal", order);
        }
    }
}
