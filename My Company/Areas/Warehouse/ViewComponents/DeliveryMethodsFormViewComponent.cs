using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Warehouse.ViewModels.PickingMethods;
using My_Company.EnumTypes;
using My_Company.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewComponents
{
    public class DeliveryMethodsFormViewComponent : ViewComponent
    {
        private readonly IConfig config;
        private readonly IRepositoryWrapper repositoryWrapper;

        public DeliveryMethodsFormViewComponent(IConfig config, IRepositoryWrapper repositoryWrapper)
        {
            this.config = config;
            this.repositoryWrapper = repositoryWrapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var configRepo = repositoryWrapper.ConfigRepository;
            var pickingMethods = await config.GetAvailavlePickingMethods(configRepo);
            List<PickingMethodViewModel> pickingMethodDtos = new();
            var values = Enum.GetValues(typeof(DeliveryType)).Cast<DeliveryType>();
            foreach (var pm in values)
            {
                var exists = pickingMethods.FirstOrDefault(p => p.Type == pm);
                pickingMethodDtos.Add(new PickingMethodViewModel
                {
                    Enabled = exists != null,
                    Price = exists == null ? null : (exists.Price / 100.0M).ToString(),
                    Type = pm
                });
            }
            var model = new PickingMethodsFormViewModel
            {
                Methods = pickingMethodDtos,
                Addres = await config.GetPersonalPickupAddress(repositoryWrapper.ConfigRepository),
            };

            return View("DeliveryMethodsForm", model);
        }
    }
}
