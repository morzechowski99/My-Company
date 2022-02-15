//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using Microsoft.AspNetCore.Mvc;
using My_Company.Models;
using System.Collections.Generic;

namespace My_Company.Areas.Shop.ViewComponents
{
    public class CategoryInfoViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(List<Category> categories)
        {
            return View("CategoryInfo", categories);
        }
    }
}
