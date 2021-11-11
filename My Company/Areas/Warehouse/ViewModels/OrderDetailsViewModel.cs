using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class OrderDetailsViewModel
    {
        [Display(Name ="Numer zamówienia")]
        public Guid Id { get; set; }
        [Display(Name = "Data zamówienia")]
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }
        [Display(Name = "Komentarz")]
        public string Comment { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
        [Display(Name = "Zamówione produkty")]
        public List<ProductOrderDetailsViewModel> ProductOrders { get; set; }
        [Display(Name = "Kompletował")]
        public string PickingUser { get; set; }
        [Display(Name = "Adres wysyłki")]
        public string Address { get; set; }
    }
}
