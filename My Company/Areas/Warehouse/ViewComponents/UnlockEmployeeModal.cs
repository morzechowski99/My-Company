﻿using Microsoft.AspNetCore.Mvc;

namespace My_Company.Areas.Warehouse.ViewComponents
{
    public class UnlockEmployeeModal : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("UnlockEmployeeModal");
        }
    }
}
