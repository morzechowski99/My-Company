//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using My_Company.EnumTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Shop.ViewModels.Order
{
    public class OrderDefailsViewModel
    {
        [Display(Name = "Numer zamówienia")]
        public Guid Id { get; set; }
        [Display(Name = "Data zamówienia")]
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
        [Display(Name = "Forma dostawy")]
        public DeliveryType DeliveryType { get; set; }
        public string DeliveryTypeString { get; set; }
        [Display(Name = "Metoda płatności")]
        public string PaymentMethodString { get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }
        [Display(Name = "Cena płatności")]
        [DataType(DataType.Currency)]
        public decimal PaymentPrice { get; set; }
        [Display(Name = "Cena dostawy")]
        [DataType(DataType.Currency)]
        public decimal DeliveryPrice { get; set; }
        public List<OrderItem> Products { get; set; }
        public AddressModel Address { get; set; }
        public OrderDeliveryViewModel Delivery { get; set; }
        [Display(Name = "Kwota")]
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }
    }
}
