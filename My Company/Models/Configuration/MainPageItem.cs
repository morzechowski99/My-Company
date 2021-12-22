using System.ComponentModel.DataAnnotations;

namespace My_Company.Models.Configuration
{
    public class MainPageItem
    {
        public int Order { get; set; }
        [Display(Name = "Kategoria docelowa")]
        public int? CategoryId { get; set; }
        [Display(Name = "Tekst na przycisku")]
        [Required]
        public string ButtonText { get; set; }
        public string PhotoUrl { get; set; }
        [Display(Name = "Tytuł")]
        [Required]
        public string Title { get; set; }
        [Display(Name = "Treść")]
        [Required]
        public string Descritpion { get; set; }
    }
}
