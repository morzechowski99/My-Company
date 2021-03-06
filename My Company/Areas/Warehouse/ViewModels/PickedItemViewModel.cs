using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class PickedItemViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Nazwa")]
        public string ProductName { get; set; }
        [Display(Name = "Kod")]
        public string EANCode { get; set; }
        [Display(Name = "Sektor")]
        public string Sector { get; set; }
        [Display(Name = "Ilość")]
        public int Count { get; set; }
    }
}
