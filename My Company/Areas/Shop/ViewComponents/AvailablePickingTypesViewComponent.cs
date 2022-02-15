//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using Microsoft.AspNetCore.Mvc;
using My_Company.Interfaces;
using System.Threading.Tasks;

namespace My_Company.Areas.Shop.ViewComponents
{
    public class AvailablePickingTypesViewComponent : ViewComponent
    {
        private IConfig config;
        private IRepositoryWrapper repositoryWrapper;

        public AvailablePickingTypesViewComponent(IConfig config, IRepositoryWrapper repositoryWrapper)
        {
            this.config = config;
            this.repositoryWrapper = repositoryWrapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var availableMethods = await config.GetAvailavlePickingMethods(repositoryWrapper.ConfigRepository);

            return View("AvailablePickingTypes", availableMethods);
        }
    }

}
