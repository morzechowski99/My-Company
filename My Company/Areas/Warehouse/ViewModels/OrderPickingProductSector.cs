//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class OrderPickingProductSector
    {
        public string SectorName { get; set; }
        [Display(Name = "Ilość")]
        public int Count { get; set; }
    }
}
