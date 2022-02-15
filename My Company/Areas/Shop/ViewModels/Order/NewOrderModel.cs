//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using My_Company.EnumTypes;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Shop.ViewModels.Order
{
    public class NewOrderModel
    {
        [Display(Name = "Adres dostawy")]
        public AddressModel ShippingAddress { get; set; }
        [Display(Name = "Rodzaj dostawy")]
        [Required]
        public DeliveryType DeliveryType { get; set; }
        [Display(Name = "Rodzaj płatności")]
        [Required]
        public PaymentMethodEnum PaymentMethod { get; set; }
        public int? AddressId { get; set; }
        public string PackLockerName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
