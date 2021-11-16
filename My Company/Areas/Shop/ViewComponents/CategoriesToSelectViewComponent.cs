using Microsoft.AspNetCore.Mvc;
using My_Company.Interfaces;
using System.Threading.Tasks;

namespace My_Company.Areas.Shop.ViewComponents
{
    public class CategoriesToSelectViewComponent : ViewComponent
    {
        private readonly IRepositoryWrapper repositoryWrapper;

        public CategoriesToSelectViewComponent(IRepositoryWrapper repositoryWrapper)
        {
            this.repositoryWrapper = repositoryWrapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? categoryId)
        {
            var categories = await repositoryWrapper.CategoriesRepository.GetChildCategoriesById(categoryId);
            ViewBag.Parent = await repositoryWrapper.CategoriesRepository.GetParentCategory(categoryId);
            ViewBag.IsMain = categoryId == null;
            return View("CategoriesToSelect", categories);
        }
    }
}
