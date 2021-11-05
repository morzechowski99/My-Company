using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class DeliveryDetailsViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Numer PZ")]
        public string PZNumber { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Data")]
        public DateTime DeliveryDate { get; set; }
        [Display(Name = "Dostawca")]
        public string Supplier { get; set; }
        public int SupplierId { get; set; }
        public bool WasCorrected { get; set; }
        public int? CorrectingId { get; set; }
        [Display(Name = "Produkty")]
        public List<DeliveryProductViewModel> Products { get; set; }
    }
}
