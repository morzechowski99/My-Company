using My_Company.Areas.Shop.ViewModels.Order;
using My_Company.EnumTypes;
using My_Company.Models;

namespace My_Company.Services.DeliveryService
{
    [DeliveryType(DeliveryType.PaczkomatyInPost)]
    public class InPostService : IDeliveryService
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
