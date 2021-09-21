using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewComponents
{
    public class DeleteCategoryModalViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("DeleteCategoryModal");
        }
    }
}
