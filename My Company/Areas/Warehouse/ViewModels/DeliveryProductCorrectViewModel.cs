using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class DeliveryProductCorrectViewModel
    {
        [Required]
        [Display(Name = "Ilość")]
        [Range(0, int.MaxValue,ErrorMessage = "Niepoprawna ilość")]
        public int Count { get; set; }
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Display(Name = "Sektor")]
        public string Sector { get; set; }
        public string Photo { get; set; }
        [Required]
        public int Id { get; set; }
        [Required]
        public int SectorId { get; set; }

    }
}
