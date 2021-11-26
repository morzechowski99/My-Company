using My_Company.Areas.Shop.ViewModels.Order;
using My_Company.Models;
using My_Company.Services.DeliveryService;

namespace My_Company.Interfaces
{
    public interface IDeliveryService
    {
        public IDeliveryStrategy Strategy { get; set; }
        OrderDelivery GetDelivery(NewOrderModel order);
    }
}
