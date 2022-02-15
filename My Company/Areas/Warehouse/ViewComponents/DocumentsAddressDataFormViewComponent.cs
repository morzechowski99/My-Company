//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using Microsoft.AspNetCore.Mvc;
using My_Company.Interfaces;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewComponents
{
    public class DocumentsAddressDataFormViewComponent : ViewComponent
    {
        private readonly IConfig config;
        private readonly IRepositoryWrapper repositoryWrapper;

        public DocumentsAddressDataFormViewComponent(IConfig config, IRepositoryWrapper repositoryWrapper)
        {
            this.config = config;
            this.repositoryWrapper = repositoryWrapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var configRepo = repositoryWrapper.ConfigRepository;
            var address = await config.GetDocumentAddress(configRepo);
            return View("DocumentsAddressDataForm", address);
        }
    }
}
