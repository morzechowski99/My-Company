//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using Microsoft.AspNetCore.Mvc;
using My_Company.Areas.Warehouse.ViewModels;
using My_Company.Helpers;
using My_Company.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewComponents
{
    public class BasicInfoConfigFormViewComponent : ViewComponent
    {
        private readonly IConfig config;
        private readonly IRepositoryWrapper repositoryWrapper;

        public BasicInfoConfigFormViewComponent(IConfig config, IRepositoryWrapper repositoryWrapper)
        {
            this.config = config;
            this.repositoryWrapper = repositoryWrapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var configRepo = repositoryWrapper.ConfigRepository;
            List<ConfigValue> configValues = new();
            configValues.Add(new ConfigValue { Key = Constants.ConfigKeys.Description, Value = await config.GetValue(Constants.ConfigKeys.Description, configRepo) });
            configValues.Add(new ConfigValue { Key = Constants.ConfigKeys.Title, Value = await config.GetValue(Constants.ConfigKeys.Title, configRepo) });
            configValues.Add(new ConfigValue { Key = Constants.ConfigKeys.Keywords, Value = await config.GetValue(Constants.ConfigKeys.Keywords, configRepo) });
            return View("BasicInfoConfigForm", configValues);
        }
    }
}
