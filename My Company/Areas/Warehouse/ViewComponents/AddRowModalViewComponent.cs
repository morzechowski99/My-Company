using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Warehouse.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
