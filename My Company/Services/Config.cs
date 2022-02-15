//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using My_Company.EnumTypes;
using My_Company.Interfaces;
using My_Company.Models.Configuration;
using My_Company.Services.DocumentGeneratorService.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static My_Company.Helpers.Constants;

namespace My_Company.Services
{
    public class Config : IConfig
    {
        private Dictionary<string, string> configDictionary;

        public Config()
        {
            configDictionary = null;
        }

        public async Task<string> GetValue(string key, IConfigRepository configRepository)
        {
            if (configDictionary == null)
                await Setup(configRepository);
            return configDictionary[key];
        }

        private async Task Setup(IConfigRepository configRepository)
        {
            configDictionary = await configRepository.GetValues();
        }

        public async Task SetValue(string key, string value, IConfigRepository configRepository)
        {
            if (configDictionary == null)
                await Setup(configRepository);
            await configRepository.SetValue(key, value);
            configDictionary[key] = value;
        }

        public async Task<List<PickingMethod>> GetAvailavlePickingMethods(IConfigRepository configRepository)
        {
            if (configDictionary == null)
                await Setup(configRepository);
            return JsonConvert.DeserializeObject<List<PickingMethod>>(configDictionary[AVAILABLE_PICKING_METHODS]);
        }

        public async Task<List<PaymentMethod>> GetAvailavlePaymentsMethods(IConfigRepository configRepository)
        {
            if (configDictionary == null)
                await Setup(configRepository);
            return JsonConvert.DeserializeObject<List<PaymentMethod>>(configDictionary[AVAILABLE_PAYMENT_METHODS]);
        }

        public async Task<int> GetShippingPrice(DeliveryType deliveryType, IConfigRepository configRepository)
        {
            var pickingMethods = await GetAvailavlePickingMethods(configRepository);
            return pickingMethods.First(m => m.Type == deliveryType).Price;
        }

        public async Task<int> GetPaymentPrice(PaymentMethodEnum paymentMethod, IConfigRepository configRepository)
        {

            var paymentsMethods = await GetAvailavlePaymentsMethods(configRepository);
            return paymentsMethods.First(m => m.Method == paymentMethod).Price;
        }

        public async Task<DataToPayment> GetDataToPayment(IConfigRepository configRepository)
        {
            return JsonConvert.DeserializeObject<DataToPayment>
                (await GetValue(ConfigKeys.DataToPayment, configRepository));
        }

        public async Task SetDataToPayment(DataToPayment dataToPayment, IConfigRepository configRepository)
        {
            await SetValue(ConfigKeys.DataToPayment, JsonConvert.SerializeObject(dataToPayment), configRepository);
        }

        public async Task<bool> IsShopEnabled(IConfigRepository configRepository)
        {
            return JsonConvert.DeserializeObject<bool>
                (await GetValue(ConfigKeys.IsShopEnabled, configRepository));
        }

        public async Task SetIsShopEnabled(bool enabled, IConfigRepository configRepository)
        {
            await SetValue(ConfigKeys.IsShopEnabled, JsonConvert.SerializeObject(enabled), configRepository);
        }

        public async Task SetPaymentsMethods(List<PaymentMethod> newPaymentsMethods, IConfigRepository configRepository)
        {
            await SetValue(AVAILABLE_PAYMENT_METHODS, JsonConvert.SerializeObject(newPaymentsMethods), configRepository);
        }

        public async Task SetPersonalPickupAddress(PersonalPickupAddress addess, IConfigRepository configRepository)
        {
            await SetValue(ConfigKeys.PersonalPickupAddress, JsonConvert.SerializeObject(addess), configRepository);
        }

        public async Task<PersonalPickupAddress> GetPersonalPickupAddress(IConfigRepository configRepository)
        {
            return JsonConvert.DeserializeObject<PersonalPickupAddress>
                (await GetValue(ConfigKeys.PersonalPickupAddress, configRepository));
        }

        public async Task SetPickingMethods(List<PickingMethod> newPickingMethods, IConfigRepository configRepository)
        {
            await SetValue(AVAILABLE_PICKING_METHODS, JsonConvert.SerializeObject(newPickingMethods), configRepository);
        }

        public async Task<AddressData> GetDocumentAddress(IConfigRepository configRepository)
        {
            return JsonConvert.DeserializeObject<AddressData>
               (await GetValue(ConfigKeys.DocumentAddress, configRepository));
        }

        public async Task SetDocumentAddress(AddressData newAddress, IConfigRepository configRepository)
        {
            await SetValue(ConfigKeys.DocumentAddress, JsonConvert.SerializeObject(newAddress), configRepository);
        }

        public async Task<List<MainPageItem>> GetMainPageContent(IConfigRepository configRepo)
        {
            return JsonConvert.DeserializeObject<List<MainPageItem>>
               (await GetValue(ConfigKeys.MainPageContent, configRepo));
        }

        public async Task SetMainPageContent(List<MainPageItem> mainPageContent, IConfigRepository configRepo)
        {
            await SetValue(ConfigKeys.MainPageContent, JsonConvert.SerializeObject(mainPageContent), configRepo);
        }
    }
}
