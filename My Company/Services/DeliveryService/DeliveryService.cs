using My_Company.Areas.Shop.ViewModels.Order;
using My_Company.Interfaces;
using My_Company.Models;

namespace My_Company.Services.DeliveryService
{
    public class DeliveryService : IDeliveryService
    {
        public IDeliveryStrategy Strategy { get; set; } = new PersonalPickupStrategy();

        public OrderDelivery GetDelivery(NewOrderModel order)
        {
            return Strategy.GetDelivery(order);
        }
    }
}
