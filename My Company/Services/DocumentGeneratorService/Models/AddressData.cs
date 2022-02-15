//Program powstał na Wydziale Informatyki Politechniki Białostockiej
using System.ComponentModel.DataAnnotations;

namespace My_Company.Services.DocumentGeneratorService.Models
{
    public class AddressData
    {
        [Display(Name = "Nazwa firmy")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "Adres 1")]
        [Required]
        public string Address1 { get; set; }
        [Display(Name = "Adres 2")]
        [Required]
        public string Address2 { get; set; }
        [Display(Name = "NIP")]
        [Required]
        [MinLength(10, ErrorMessage = "NIP musi mieć 10 znaków")]
        [MaxLength(10, ErrorMessage = "NIP musi mieć 10 znaków")]
        [RegularExpression(@"^\d+$")]
        public string NIP { get; set; }
        [Required]
        [Display(Name = "Miejsce wystawienia dokumentu")]
        public string DocumentPlace { get; set; }

    }
}
