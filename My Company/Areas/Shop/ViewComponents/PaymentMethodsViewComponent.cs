//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using Microsoft.AspNetCore.Mvc;
using My_Company.Interfaces;
using System.Threading.Tasks;

namespace My_Company.Areas.Shop.ViewComponents
{
    public class PaymentMethodsViewComponent : ViewComponent
    {
        private readonly IConfig config;
        private readonly IRepositoryWrapper repositoryWrapper;

        public PaymentMethodsViewComponent(IConfig config, IRepositoryWrapper repositoryWrapper)
        {
            this.config = config;
            this.repositoryWrapper = repositoryWrapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var availableMethods = await config.GetAvailavlePaymentsMethods(repositoryWrapper.ConfigRepository);

            return View("PaymentMethods", availableMethods);
        }
    }
}
