using Microsoft.AspNetCore.Mvc;

namespace My_Company.Areas.Warehouse.ViewComponents
{
    public class DeleteSupplierModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("DeleteSupplierModal");
        }
    }
}
