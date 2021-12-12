using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewComponents
{
    public class MainPageFormViewComponent : ViewComponent
    {
        private readonly IConfig config;
        private readonly IRepositoryWrapper repositoryWrapper;
        private readonly IMapper mapper;

        public MainPageFormViewComponent(IConfig config, IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            this.config = config;
            this.repositoryWrapper = repositoryWrapper;
            this.mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var configRepo = repositoryWrapper.ConfigRepository;
            var mainPageContent = await config.GetMainPageContent(configRepo);
            var mainPageContentView = mapper.Map<List<MainPageItemViewModel>>(mainPageContent).OrderBy(i => i.Order).ToList();

            foreach(var item in mainPageContentView)
            {
                var categoryId = mainPageContent.First(i => i.Order == item.Order).CategoryId;
                item.CategoryName = categoryId.HasValue ? 
                    await repositoryWrapper.CategoriesRepository.GetCategoryTreeWithCategoryName
                    (await repositoryWrapper.CategoriesRepository.GetById(categoryId.Value)) : "Wszystkie produkty";
            }

            return View("MainPageForm", mainPageContentView);
        }
    }
}
