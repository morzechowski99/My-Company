using My_Company.EnumTypes;
using My_Company.Interfaces;
using My_Company.Models.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
            return JsonSerializer.Deserialize<List<PickingMethod>>(configDictionary[AVAILABLE_PICKING_METHODS]);
        }

        public async Task<List<PaymentMethod>> GetAvailavlePaymentsMethods(IConfigRepository configRepository)
        {
            if (configDictionary == null)
                await Setup(configRepository);
            return JsonSerializer.Deserialize<List<PaymentMethod>>(configDictionary[AVAILABLE_PAYMENT_METHODS]);
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
            return JsonSerializer.Deserialize<DataToPayment>
                (await GetValue(ConfigKeys.DataToPayment, configRepository));
        }

        public async Task SetDataToPayment(DataToPayment dataToPayment, IConfigRepository configRepository)
        {
            await SetValue(ConfigKeys.DataToPayment, JsonSerializer.Serialize(dataToPayment),configRepository);
        }
    }
}
