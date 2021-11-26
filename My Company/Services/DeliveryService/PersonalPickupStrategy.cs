using My_Company.Areas.Shop.ViewModels.Order;
using My_Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Services.DeliveryService
{
    public class PersonalPickupStrategy : IDeliveryStrategy
    {
        public OrderDelivery GetDelivery(NewOrderModel order)
        {
            PersonalPickup delivery = new PersonalPickup();

            return delivery;
        }

    }
}
