//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Helpers;
using My_Company.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewComponents
{
    public class ContentConfigFormViewComponent : ViewComponent
    {
        private readonly IConfig config;
        private readonly IRepositoryWrapper repositoryWrapper;

        public ContentConfigFormViewComponent(IConfig config, IRepositoryWrapper repositoryWrapper)
        {
            this.config = config;
            this.repositoryWrapper = repositoryWrapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var configRepo = repositoryWrapper.ConfigRepository;
            List<ConfigValue> configValues = new();
            configValues.Add(new ConfigValue { Key = Constants.ConfigKeys.CartSubtitle, Value = await config.GetValue(Constants.ConfigKeys.CartSubtitle, configRepo) });
            configValues.Add(new ConfigValue { Key = Constants.ConfigKeys.OrderConfirmText, Value = await config.GetValue(Constants.ConfigKeys.OrderConfirmText, configRepo) });
            return View("ContentConfigForm", configValues);
        }
    }
}
