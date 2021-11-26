using Microsoft.AspNetCore.Mvc;
using My_Company.Interfaces;
using System.Threading.Tasks;

namespace My_Company.Areas.Shop.ViewComponents
{
    public class WarehouseAddress : ViewComponent
    {
        private readonly IRepositoryWrapper repositoryWrapper;

        public WarehouseAddress(IRepositoryWrapper repositoryWrapper)
        {
            this.repositoryWrapper = repositoryWrapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var warehouse = await repositoryWrapper.WarehouseRepository.GetWarehouse();
            return View("WarehouseAddress", warehouse);
        }
    }
}
