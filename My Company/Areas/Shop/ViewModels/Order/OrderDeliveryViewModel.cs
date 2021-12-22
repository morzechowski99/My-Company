using My_Company.EnumTypes;
using My_Company.Models.InPostModels;

namespace My_Company.Areas.Shop.ViewModels.Order
{
    public class OrderDeliveryViewModel
    {
        public DeliveryType Type { get; set; }
        public string PackLockerName { get; set; }
        public ParcelLockerInfo ParcelLockerInfo { get; set; }
    }

}
