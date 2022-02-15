//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using Microsoft.AspNetCore.Mvc;

namespace My_Company.Areas.Warehouse.ViewComponents
{
    public class LockEmployeeModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("LockEmployeeModal");
        }
    }
}
