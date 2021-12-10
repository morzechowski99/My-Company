using My_Company.EnumTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class OrderDetailsViewModel
    {
        [Display(Name = "Numer zamówienia")]
        public Guid Id { get; set; }
        [Display(Name = "Data zamówienia")]
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }
        [Display(Name = "Komentarz")]
        public string Comment { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
        public OrderStatus OrderStatus { get; set; }
        [Display(Name = "Opłacone?")]
        public bool Paid { get; set; }
        [Display(Name = "Zamówione produkty")]
        public List<ProductOrderDetailsViewModel> ProductOrders { get; set; }
        [Display(Name = "Kompletował")]
        public string PickingUser { get; set; }
        [Display(Name = "Adres wysyłki")]
        public string Address { get; set; }
        [Display(Name = "Numer WZ")]
        public string WZNumber { get; set; }
        [Display(Name = "Metoda płatności")]
        public string PaymentMethod { get; set; }
        [Display(Name = "Metoda dostawy")]
        public string DeliveryMethod { get; set; }
        [Display(Name = "Paczkomat")]
        public string ParcelLockerInfo { get; set; }
    }
}
