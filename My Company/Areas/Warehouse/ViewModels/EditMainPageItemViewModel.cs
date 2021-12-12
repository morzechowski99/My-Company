using System.ComponentModel.DataAnnotations;

namespace My_Company.Areas.Warehouse.ViewModels
{
    public class EditMainPageItemViewModel
    {
        public int Order { get; set; }
        [Display(Name = "Kategoria docelowa")]
        public int? CategoryId { get; set; }
        [Display(Name = "Tekst na przycisku")]
        [Required]
        public string ButtonText { get; set; }
        [Display(Name = "Tytuł")]
        [Required]
        public string Title { get; set; }
        [Display(Name = "Treść")]
        [Required]
        public string Descritpion { get; set; }
    }
}
