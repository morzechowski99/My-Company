//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Interfaces;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewComponents
{
    public class EditMainPageItemModalViewComponent : ViewComponent
    {
        private readonly IMapper mapper;
        private readonly IRepositoryWrapper repositoryWrapper;

        public EditMainPageItemModalViewComponent(IMapper mapper, IRepositoryWrapper repositoryWrapper)
        {
            this.mapper = mapper;
            this.repositoryWrapper = repositoryWrapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(MainPageItemViewModel item)
        {
            ViewData["CategoryId"] = new SelectList(await repositoryWrapper.CategoriesRepository.GetCategoriesTree(), "Id", "Tree");
            var itemDto = mapper.Map<EditMainPageItemViewModel>(item);
            return View("EditMainPageItemModal", itemDto);
        }
    }
}
