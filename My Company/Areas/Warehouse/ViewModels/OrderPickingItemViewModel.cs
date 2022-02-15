//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class OrderPickingItemViewModel
    {
        public int ProductOrderId { get; set; }
        [Display(Name = "Zamówiono")]
        public int Count { get; set; }
        [Display(Name = "Zebrano")]
        public int Completed { get; set; }
        public ProductListItemViewModel Product { get; set; }
        [Display(Name = "Lokacje")]
        public List<OrderPickingProductSector> ProductSectors { get; set; }
    }
}
