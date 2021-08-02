using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Warehouse.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewComponents
{
    public class AddSectorsModalViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("AddSectorsModal", new AddSectorsViewModel());
        }
    }
}
