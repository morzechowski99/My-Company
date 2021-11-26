using My_Company.Areas.Shop.ViewModels.Cart;
using My_Company.Models.InPostModels;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Shop.ViewModels.Order
{
    public class OrderSummaryViewModel
    {
        public NewOrderModel Order { get; set; }
        public Cart.Cart Cart { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Do zapłaty")]
        public decimal Total { get; set; }  
        [DataType(DataType.Currency)]
        public decimal ShippingValue { get; set; } 
        [DataType(DataType.Currency)]      
        public decimal PaymentValue { get; set; }
        [Display(Name = "Wybrany paczkomat")]
        public ParcelLockerInfo ParcelLocker { get; set; }
    }
}
