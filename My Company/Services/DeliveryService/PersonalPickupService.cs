using My_Company.Areas.Shop.ViewModels.Order;
using My_Company.EnumTypes;
using My_Company.Models;

namespace My_Company.Services.DeliveryService
{
    [DeliveryType(DeliveryType.PersonalPickup)]
    public class PersonalPickupService : IDeliveryService
    {
        public OrderDelivery GetDelivery(NewOrderModel order)
        {
            PersonalPickup delivery = new PersonalPickup();

            return delivery;
        }

    }
}
