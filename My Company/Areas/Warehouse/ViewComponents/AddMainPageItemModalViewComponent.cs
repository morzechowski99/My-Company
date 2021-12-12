using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using My_Company.Interfaces;
using My_Company.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewComponents
{
    public class AddMainPageItemModalViewComponent : ViewComponent
    {
        private readonly IRepositoryWrapper repositoryWrapper;

        public AddMainPageItemModalViewComponent(IRepositoryWrapper repositoryWrapper)
        {
            this.repositoryWrapper = repositoryWrapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewData["CategoryId"] = new SelectList(await repositoryWrapper.CategoriesRepository.GetCategoriesTree(), "Id", "Tree");
            return View("AddMainPageItemModal", new MainPageItem());
        }
    }
}
