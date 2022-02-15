//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using My_Company.Areas.Shop.ViewModels.Order;
using My_Company.Models;

namespace My_Company.Services.DeliveryService
{
    public interface IDeliveryService
    {
        OrderDelivery GetDelivery(NewOrderModel order);
    }
}