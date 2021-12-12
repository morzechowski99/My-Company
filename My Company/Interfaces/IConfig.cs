using My_Company.EnumTypes;
using My_Company.Models.Configuration;
using My_Company.Services.DocumentGeneratorService.Models;
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
        Task<List<MainPageItem>> GetMainPageContent(IConfigRepository configRepo);
        Task<DataToPayment> GetDataToPayment(IConfigRepository configRepository);
        Task<AddressData> GetDocumentAddress(IConfigRepository configRepo);
        Task SetDataToPayment(DataToPayment dataToPayment, IConfigRepository configRepository);
        Task<bool> IsShopEnabled(IConfigRepository configRepository);
        Task SetIsShopEnabled(bool enabled,IConfigRepository configRepository);
        Task SetPaymentsMethods(List<PaymentMethod> newPaymentsMethods, IConfigRepository configRepository); 
        Task SetPersonalPickupAddress(PersonalPickupAddress addess,IConfigRepository configRepository);
        Task<PersonalPickupAddress> GetPersonalPickupAddress(IConfigRepository configRepository);
        Task SetPickingMethods(List<PickingMethod> newPickingMethods, IConfigRepository configRepository);
        Task SetDocumentAddress(AddressData newAddress, IConfigRepository configRepository);
        Task SetMainPageContent(List<MainPageItem> mainPageContent, IConfigRepository configRepo);
    }
}
