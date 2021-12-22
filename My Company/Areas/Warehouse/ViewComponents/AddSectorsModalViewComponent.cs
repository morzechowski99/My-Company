using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Warehouse.ViewModels;

namespace My_Company.Areas.Warehouse.ViewComponents
{
    public class AddSectorsModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("AddSectorsModal", new AddSectorsViewModel());
        }
    }
}
