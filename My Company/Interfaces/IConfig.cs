using My_Company.EnumTypes;
using My_Company.Models.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My_Company.Interfaces
{
    public interface IConfig
    {
        Task<string> GetValue(string key, IConfigRepository configRepository);
        Task SetValue(string key, string value, IConfigRepository configRepository);
        Task<List<PickingMethod>> GetAvailavlePickingMethods(IConfigRepository configRepository);
        Task<List<PaymentMethod>> GetAvailavlePaymentsMethods(IConfigRepository configRepository);
        Task<int> GetShippingPrice(DeliveryType deliveryType, IConfigRepository configRepository);
        Task<int> GetPaymentPrice(PaymentMethodEnum paymentMethod, IConfigRepository configRepository);
        Task<DataToPayment> GetDataToPayment(IConfigRepository configRepository);
        Task SetDataToPayment(DataToPayment dataToPayment, IConfigRepository configRepository);
    }
}
