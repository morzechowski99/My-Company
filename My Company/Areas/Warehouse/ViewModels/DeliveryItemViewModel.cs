//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class DeliveryItemViewModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int SectorId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Count { get; set; }
    }
}