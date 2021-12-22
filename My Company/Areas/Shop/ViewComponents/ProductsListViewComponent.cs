using Microsoft.AspNetCore.Mvc;
using My_Company.Interfaces;
using System.Threading.Tasks;

namespace My_Company.Areas.Shop.ViewComponents
{
    public class ProductsListViewComponent : ViewComponent
    {
        private readonly IRepositoryWrapper repositoryWrapper;

        public ProductsListViewComponent(IRepositoryWrapper repositoryWrapper)
        {
            this.repositoryWrapper = repositoryWrapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? categoryId)
        {
            var categories = await repositoryWrapper.CategoriesRepository.GetAllCategoriesInTree(categoryId);
            return View("ProductsList", categories);
        }
    }
}
