using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using My_Company.Areas.Warehouse.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using My_Company.Dictionaries;

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
