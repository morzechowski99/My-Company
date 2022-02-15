//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using Microsoft.AspNetCore.Mvc;
using My_Company.Interfaces;
using My_Company.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My_Company.Areas.Shop.ViewComponents
{
    public class FiltersViewComponent : ViewComponent
    {
        private readonly IRepositoryWrapper repositoryWrapper;

        public FiltersViewComponent(IRepositoryWrapper repositoryWrapper)
        {
            this.repositoryWrapper = repositoryWrapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? categoryId)
        {
            IEnumerable<Attribute> attributes = null;
            if (categoryId.HasValue)
                attributes = await repositoryWrapper.CategoriesRepository.GetAllAttributesByCategoryIdWithDictionaryValues(categoryId.Value);
            else
                attributes = new List<Attribute>();

            return View("Filters", attributes);
        }
    }
}
