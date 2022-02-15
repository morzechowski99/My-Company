//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class DeliveryEditViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Numer PZ")]
        public string PZNumber { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Data dostawy")]
        public DateTime DeliveryDate { get; set; }
        [Display(Name = "Dostawca")]
        public string Supplier { get; set; }
        public int SupplierId { get; set; }
        [Display(Name = "Produkty")]
        public List<DeliveryProductCorrectViewModel> Products { get; set; }
    }
}
