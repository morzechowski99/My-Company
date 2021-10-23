using Microsoft.AspNetCore.Mvc;

namespace My_Company.Areas.Warehouse.Controllers
{
    [Area("Warehouse")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
