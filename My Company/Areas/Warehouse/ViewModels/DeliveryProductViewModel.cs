using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class DeliveryProductViewModel
    {
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Display(Name = "Ilość")]
        public int Count { get; set; }
        [Display(Name = "Sektor")]
        public string Sector { get; set; }
        public string Photo { get; set; }
    }
}
