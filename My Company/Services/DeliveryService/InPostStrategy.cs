using My_Company.Areas.Shop.ViewModels.Order;
using My_Company.Models;

namespace My_Company.Services.DeliveryService
{
    public class InPostStrategy : IDeliveryStrategy
    {
        public OrderDelivery GetDelivery(NewOrderModel order)
        {
            InPostDelivery delivery = new InPostDelivery
            {
                PackLockerName = order.PackLockerName
            };
            return delivery;
        }
    }
}
