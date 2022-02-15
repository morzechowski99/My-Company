//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class CorrectedDeliveryViewModel
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
        public string CorrectedNumber { get; set; }
        public int CorrectedId { get; set; }
        [Display(Name = "Produkty")]
        public List<DeliveryCorrectedProductViewModel> Products { get; set; }
    }
}
