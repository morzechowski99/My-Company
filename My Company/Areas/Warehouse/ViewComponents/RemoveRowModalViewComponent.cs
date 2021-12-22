using Microsoft.AspNetCore.Mvc;

namespace My_Company.Areas.Warehouse.ViewComponents
{
    public class RemoveRowModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("RemoveRowModal");
        }
    }
}
