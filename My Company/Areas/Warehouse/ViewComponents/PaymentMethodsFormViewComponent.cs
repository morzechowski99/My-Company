using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Warehouse.ViewModels.PaymentMethods;
using My_Company.EnumTypes;
using My_Company.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static My_Company.Helpers.Constants;

namespace My_Company.Areas.Warehouse.ViewComponents
{
    public class PaymentMethodsFormViewComponent : ViewComponent
    {

        private readonly IConfig config;
        private readonly IRepositoryWrapper repositoryWrapper;

        public PaymentMethodsFormViewComponent(IConfig config, IRepositoryWrapper repositoryWrapper)
        {
            this.config = config;
            this.repositoryWrapper = repositoryWrapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var configRepo = repositoryWrapper.ConfigRepository;
            var paymentsMethods = await config.GetAvailavlePaymentsMethods(configRepo);
            var dataToPayment = await config.GetDataToPayment(configRepo);
            var dotPayPin = await config.GetValue(ConfigKeys.DotPayKeys.Pin, configRepo);
            var dotPayId = await config.GetValue(ConfigKeys.DotPayKeys.Id, configRepo);
            List<PaymentMethodViewModel> paymentMethodDtos = new List<PaymentMethodViewModel>();
            var values = Enum.GetValues(typeof(PaymentMethodEnum)).Cast<PaymentMethodEnum>();
            foreach (var pm in values)
            {
                var exists = paymentsMethods.FirstOrDefault(p => p.Method == pm);
                paymentMethodDtos.Add(new PaymentMethodViewModel
                {
                    Enabled = exists != null,
                    Price = exists == null ? null : (exists.Price / 100.0M).ToString(),
                    Method = pm
                });
            }
            var model = new PaymentMethodsFormViewModel
            {
                Methods = paymentMethodDtos,
                DataToPayment = dataToPayment,
                DotPayId = dotPayPin == null || dotPayPin == "" ? null : int.Parse(dotPayId),
                DotPayPin = dotPayPin
            };

            return View("PaymentMethodsForm", model);
        }
    }
}
